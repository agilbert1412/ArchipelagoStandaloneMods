using Newtonsoft.Json.Linq;
using RealLifeViewerEvents.EventsExecution;
using ViewerEventsLibrary.Events;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.Events
{
    public class QueuedEvent
    {
        private ViewerEvent _baseEvent;
        public string baseEventName;
        public int queueCount; //how many of these should be called
        public string username; //who triggered it
        public ulong userId; //who triggered it
        public string[] parameters;
        public double currentMultiplier;

        public QueuedEvent(ViewerEvent e, string[] args, double eventsCurrentMultiplier)
        {
            _baseEvent = e;
            baseEventName = e.name;
            parameters = args;
            currentMultiplier = eventsCurrentMultiplier;
        }

        public QueuedEvent(JObject data, EventCollection allEvents)
        {
            baseEventName = data["baseEventName"].ToString();
            queueCount = int.Parse(data["queueCount"].ToString());
            username = data["username"].ToString();
            userId = ulong.Parse(data["userId"].ToString());
            parameters = data["parameters"].ToObject<string[]>();
            _baseEvent = allEvents.GetEvent(baseEventName);
            currentMultiplier = data.ContainsKey("currentMultiplier") ? double.Parse(data["currentMultiplier"].ToString()) : allEvents.CurrentMultiplier;
        }

        public ViewerEvent BaseEvent => _baseEvent;

        public ExecutableEvent GetExecutableEvent(IViewerEventsLogger logger)
        {
            return new EventsFactory().GetExecutableEvent(logger, this);
        }

        public bool ValidateParameters(IViewerEventsLogger logger, out string errorMessage)
        {
            return GetExecutableEvent(logger).ValidateParameters(out errorMessage);
        }
    }
}
