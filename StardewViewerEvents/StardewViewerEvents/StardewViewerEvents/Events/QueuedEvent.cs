using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewViewerEvents.EventsExecution;

namespace StardewViewerEvents.Events
{
    public class QueuedEvent
    {
        private ViewerEvent _baseEvent;
        public string baseEventName;
        public int queueCount; //how many of these should be called
        public string username; //who triggered it
        public ulong userId; //who triggered it
        public string[] parameters;

        public QueuedEvent(ViewerEvent e, string[] args)
        {
            _baseEvent = e;
            baseEventName = e.name;
            parameters = args;
        }

        public QueuedEvent(JObject data, EventCollection allEvents)
        {
            baseEventName = data["baseEventName"].ToString();
            queueCount = int.Parse(data["queueCount"].ToString());
            username = data["username"].ToString();
            userId = ulong.Parse(data["userId"].ToString());
            parameters = data["parameters"].ToObject<string[]>();
            _baseEvent = allEvents.GetEvent(baseEventName);
        }

        public ViewerEvent BaseEvent => _baseEvent;

        public ExecutableEvent GetExecutableEvent(IMonitor logger, IModHelper modHelper)
        {
            return BaseEvent.GetExecutableEvent(logger, modHelper, this);
        }

        public bool ValidateParameters(IMonitor logger, IModHelper modHelper, out string errorMessage)
        {
            return GetExecutableEvent(logger, modHelper).ValidateParameters(out errorMessage);
        }
    }
}
