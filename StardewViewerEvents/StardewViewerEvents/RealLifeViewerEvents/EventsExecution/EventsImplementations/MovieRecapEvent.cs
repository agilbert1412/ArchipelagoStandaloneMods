using RealLifeViewerEvents.Events;
using RealLifeViewerEvents.Events.Constants;
using RealLifeViewerEvents.Extensions;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class MovieRecapEvent : ManualEndEvent
    {

        public MovieRecapEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (QueuedEvent.parameters == null || QueuedEvent.parameters.Length < 1)
            {
                errorMessage = "You must specify the title of a movie or show";
                return false;
            }

            errorMessage = "";
            return true;
        }
    }
}
