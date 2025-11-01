using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents
{
    public class RandomWeatherEvent : WeatherEvent
    {

        public RandomWeatherEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        protected override string GetNewWeather()
        {
            var newWeather = _validWeathers[Game1.random.Next(_validWeathers.Length)];
            return newWeather;
        }
    }
}
