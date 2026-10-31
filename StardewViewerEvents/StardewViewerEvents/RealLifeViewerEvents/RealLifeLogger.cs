using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents
{
    public class RealLifeLogger : IViewerEventsLogger
    {
        public void LogError(string message)
        {
            Log("ERROR", message);
        }

        public void LogError(string message, Exception exception)
        {
            Log("ERROR", $"{message}{Environment.NewLine}\tException Message: {exception.Message}{Environment.NewLine}\tStack Trace: {exception.StackTrace}");
        }

        public void LogWarning(string message)
        {
            Log("WARNING", message);
        }

        public void LogDebug(string message)
        {
            Log("DEBUG", message);
        }

        public void LogInfo(string message)
        {
            Log("INFO", message);
        }

        private void Log(string level, string message)
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()} - [{level.ToUpper()}]: {message}");
        }
    }
}
