using Discord.WebSocket;
using ViewerEventsLibrary.Events;

namespace ViewerEventsLibrary
{
    public interface IEventQueue<T> : IEnumerable<T>
    {
        bool IsEmpty { get; }
        bool Any { get; }
        int Count { get; }
        T First { get; }
        bool IsPaused();
        void RemoveFirst();
        void PrintToConsole();
        void QueueEvent(T eventToSend);
        void PushAtBeginning(ViewerEvent forcedEvent, string[] parameters, SocketUserMessage message, int count);
        void Pause();
        void Unpause();
        void ImportFrom(string combine, EventCollection eventsExecutorEvents);
        void ExportTo(string combine);
    }
}
