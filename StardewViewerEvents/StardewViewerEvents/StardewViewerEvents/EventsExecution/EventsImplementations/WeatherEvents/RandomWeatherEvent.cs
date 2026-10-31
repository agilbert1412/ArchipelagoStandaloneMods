using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents
{
    public class RandomWeatherEvent : WeatherEvent
    {

        public RandomWeatherEvent(IViewerEventsLogger logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override string GetNewWeather()
        {
            var newWeather = _validWeathers[Game1.random.Next(_validWeathers.Length)];
            return newWeather;
        }
    }
}
