using StardewModdingAPI;

namespace StardewViewerEvents.Extensions
{
    public static class IMonitorExtensions
    {
        public static void LogDebug(this IMonitor monitor, string text)
        {
            monitor.Log(text, LogLevel.Debug);
        }

        public static void LogInfo(this IMonitor monitor, string text)
        {
            monitor.Log(text, LogLevel.Info);
        }

        public static void LogWarning(this IMonitor monitor, string text)
        {
            monitor.Log(text, LogLevel.Warn);
        }

        public static void LogError(this IMonitor monitor, string text)
        {
            monitor.Log(text, LogLevel.Error);
        }

        public static void LogError(this IMonitor monitor, string text, Exception e)
        {
            monitor.Log($"{text}. Exception: {e}", LogLevel.Error);
        }
    }
}