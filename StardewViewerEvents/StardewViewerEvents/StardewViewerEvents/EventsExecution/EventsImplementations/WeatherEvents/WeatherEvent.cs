using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.WeatherEvents
{
    public abstract class WeatherEvent : ExecutableEvent
    {
        protected static readonly string[] _validWeathers = new[] { "Sun", "Rain", "Storm", "Snow", "GreenRain", "Wind", "Festival", "Wedding" };

    protected WeatherEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var newWeather = GetNewWeather();
            Game1.isRaining = false;
            Game1.isSnowing = false;
            Game1.isLightning = false;
            Game1.isDebrisWeather = false;
            Game1.isGreenRain = false;
            switch (newWeather)
            {
                case "Sun":
                    break;
                case "Rain":
                    Game1.isRaining = true;
                    break;
                case "Storm":
                    Game1.isRaining = true;
                    Game1.isLightning = true;
                    break;
                case "Snow":
                    Game1.isSnowing = true;
                    break;
                case "GreenRain":
                    Game1.isGreenRain = true;
                    // internal static bool wasGreenRain = false;
                    var wasGreenRainField = _modHelper.Reflection.GetField<bool>(typeof(Game1), "wasGreenRain");
                    wasGreenRainField.SetValue(true);
                    break;
                case "Wind":
                    Game1.isDebrisWeather = true;
                    break;
                case "Festival":
                case "Wedding":
                    break;

            }
            Game1.weatherForTomorrow = newWeather;
            Game1.updateWeather(Game1.currentGameTime);
        }

        protected abstract string GetNewWeather();
    }
}
