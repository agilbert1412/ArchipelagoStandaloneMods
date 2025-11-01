using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents
{
    public class SpecificWeatherEvent : WeatherEvent
    {

        public SpecificWeatherEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters()
        {
            if (!base.ValidateParameters())
            {
                return false;
            }

            var desiredWeather = GetSingleParameter();
            return _validWeathers.Any(x => x.Equals(desiredWeather, StringComparison.InvariantCultureIgnoreCase));
        }

        protected override string GetNewWeather()
        {
            var desiredWeather = GetSingleParameter();
            var newWeather = _validWeathers.First(x => x.Equals(desiredWeather, StringComparison.InvariantCultureIgnoreCase));
            return newWeather;
        }
    }
}