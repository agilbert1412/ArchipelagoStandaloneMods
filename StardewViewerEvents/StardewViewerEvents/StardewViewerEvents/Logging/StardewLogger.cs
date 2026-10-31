using StardewModdingAPI;
using StardewViewerEvents.Extensions;
using ViewerEventsLibrary.Logging;

namespace StardewViewerEvents.Logging
{
    public class StardewLogger : IViewerEventsLogger
    {
        private IMonitor _monitor;

        public StardewLogger(IMonitor monitor)
        {
            _monitor = monitor;
        }

        public void LogError(string message)
        {
            _monitor.LogError(message);
        }

        public void LogWarning(string message)
        {
            _monitor.LogWarning(message);
        }

        public void LogDebug(string message)
        {
            _monitor.LogDebug(message);
        }

        public void LogInfo(string message)
        {
            _monitor.LogInfo(message);
        }

        public void LogError(string message, Exception exception)
        {
            _monitor.LogError(message, exception);
        }
    }
}
