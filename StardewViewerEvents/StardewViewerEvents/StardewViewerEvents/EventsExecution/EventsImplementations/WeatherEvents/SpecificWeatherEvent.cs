using StardewModdingAPI;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents
{
    public class SpecificWeatherEvent : WeatherEvent
    {

        public SpecificWeatherEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (!base.ValidateParameters(out errorMessage))
            {
                return false;
            }

            var desiredWeather = GetSingleParameter();
            errorMessage =
                $"Unrecognized weather [{desiredWeather}]. Must choose one of the following weathers: [{string.Join(",", _validWeathers)}]";
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