using ViewerEventsLibrary.Events;

namespace ViewerEventsLibrary
{
    public interface IEventsGenerator
    {
        List<ViewerEvent> GenerateDefaultEvents();
    }
}
