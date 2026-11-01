using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RealLifeViewerEvents.EventsExecution;
using System.Collections;
using System.Text;
using ViewerEventsLibrary;
using ViewerEventsLibrary.Events;
using ViewerEventsLibrary.Extensions;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.Events
{
    public class EventQueue : IEventQueue<QueuedEvent>
    {
        private readonly IViewerEventsLogger _logger;
        private readonly SoundPlayer _soundPlayer;
        public List<QueuedEvent> _eventQueue;
        private bool _globalPause;

        public EventQueue(IViewerEventsLogger logger, SoundPlayer soundPlayer)
        {
            _logger = logger;
            _soundPlayer = soundPlayer;
            _eventQueue = new List<QueuedEvent>();
            _globalPause = false;
        }

        public bool IsEmpty => !_eventQueue.Any();
        public bool Any => _eventQueue.Any();
        public int Count => _eventQueue.Count;

        public QueuedEvent First => _eventQueue.First();

        public void ImportFrom(string eventsQueueFile, EventCollection allEvents)
        {
            if (!File.Exists(eventsQueueFile))
            {
                ExportTo(eventsQueueFile);
                return;
            }

            var lines = File.ReadAllText(eventsQueueFile, Encoding.UTF8);
            dynamic jsonData = JsonConvert.DeserializeObject(lines);
            foreach (JObject ttgEventString in jsonData)
            {
                var ttgEvent = new QueuedEvent(ttgEventString, allEvents);
                _eventQueue.Add(ttgEvent);
            }
        }

        public void ExportTo(string eventsQueueFile)
        {
            var json = JsonConvert.SerializeObject(_eventQueue, Formatting.Indented);
            File.WriteAllText(eventsQueueFile, json);
        }

        public void PushAtBeginning(ViewerEvent forcedEvent, string[] parameters, SocketUserMessage message, int count)
        {
            var queuedForcedEvent = new QueuedEvent(forcedEvent, parameters);
            queuedForcedEvent.queueCount = 1;
            queuedForcedEvent.username = message.Author.GetDisplayName();
            queuedForcedEvent.userId = message.Author.Id;
            PushAtBeginning(queuedForcedEvent);
        }

        public void PushAtBeginning(QueuedEvent eventQueued)
        {
            _eventQueue.Insert(0, eventQueued);
        }

        public void QueueEvent(QueuedEvent eventQueued)
        {
            _eventQueue.Add(eventQueued);
        }

        public void RemoveFirst()
        {
            _eventQueue.RemoveAt(0);
        }

        public void Clear()
        {
            _eventQueue.Clear();
        }

        public void OnEventAdded()
        {
            _soundPlayer.PlayNewEventSound();
            PrintNextEvent();
            PrintNextEventCommand();
        }

        public void PrintNextEventCommand()
        {
            if (IsEmpty)
            {
                return;
            }

            Console.WriteLine($"Use '{ViewerEventsExecutor.GO_COMMAND}' or '{ViewerEventsExecutor.SKIP_COMMAND}'");
        }

        public void PrintToConsole()
        {
            _logger.LogInfo($"Queue current has {_eventQueue.Count} Events:{Environment.NewLine}{GetPrintableQueue()}");
        }

        public void PrintNextEvent()
        {
            if (IsEmpty)
            {
                return;
            }

            _logger.LogInfo($"Event Available: {GetPrintableEvent(First)}");
        }

        private string GetDiscordMessage()
        {
            var message = $"Queue current has {_eventQueue.Count} Events:\n```";
            message += GetPrintableQueue();
            message += "\n```";
            return message;
        }

        private string GetPrintableQueue()
        {
            var printedQueue = "";
            for (var i = 0; i < _eventQueue.Count; i++)
            {
                var queuedEvent = _eventQueue[i];
                printedQueue += $"\t[{i}]{GetPrintableEvent(queuedEvent)}{Environment.NewLine}";
            }

            return printedQueue;
        }

        private string GetPrintableEvent(QueuedEvent eventToPrint)
        {
            var text = $"Event: {eventToPrint.baseEventName}";
            if (eventToPrint.parameters != null && eventToPrint.parameters.Length > 0)
            {
                foreach (var parameter in eventToPrint.parameters)
                {
                    text += $" {parameter}";
                }
            }
            if (eventToPrint.queueCount > 1)
            {
                text += $"\t (x{eventToPrint.queueCount})";
            }
            return text;
        }

        public IEnumerator<QueuedEvent> GetEnumerator()
        {
            return _eventQueue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool IsPaused()
        {
            return _globalPause;
        }

        public void Pause()
        {
            _globalPause = true;
        }

        public void Unpause()
        {
            _globalPause = false;
        }
    }
}
