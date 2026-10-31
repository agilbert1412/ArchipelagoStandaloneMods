using Discord.WebSocket;
using ViewerEventsLibrary.Events;

namespace ViewerEventsLibrary
{
    public interface IEventsExecutor<T>
    {
        EventCollection Events { get; }
        IEventQueue<T> Queue { get; }
        void AddOrIncrementEventInQueue(SocketUser messageAuthor, ViewerEvent chosenEvent, string[] parameters);
        void AddEventToQueue(SocketUser messageAuthor, ViewerEvent viewerEvent, string[] parameters);
    }
}
