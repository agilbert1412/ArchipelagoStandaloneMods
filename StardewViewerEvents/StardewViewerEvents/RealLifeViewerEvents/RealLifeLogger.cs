using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents
{
    public class RealLifeLogger : IViewerEventsLogger
    {
        private const string _logPath = "RealLifeViewerEventslog.txt";

        public RealLifeLogger()
        {
            File.Delete(_logPath);
        }

        public void LogDebug(string message)
        {
            LogToConsole(LogLevel.Debug, message);
            LogToFile(LogLevel.Debug, message);
        }

        public void LogError(string message)
        {
            LogToConsole(LogLevel.Error, message);
            LogToFile(LogLevel.Error, message);
        }

        public void LogError(string message, Exception exception)
        {
            LogError($"{message}{Environment.NewLine}\tException Message: {exception.Message}{Environment.NewLine}\tStack Trace: {exception.StackTrace}");
        }

        public void LogError(Exception exception)
        {
            LogError($"Exception Message: {exception.Message}{Environment.NewLine}\tStack Trace: {exception.StackTrace}");
        }

        public void LogWarning(string message)
        {
            LogToConsole(LogLevel.Warning, message);
            LogToFile(LogLevel.Warning, message);
        }

        public void LogInfo(string message)
        {
            LogToConsole(LogLevel.Info, message);
            LogToFile(LogLevel.Info, message);
        }

        private string GetMessageWithTimestamp(LogLevel level, string message)
        {
            return $"{DateTime.Now.ToLongTimeString()} - [{level.ToString().ToUpper()}]: {message}";
        }

        private void LogToConsole(LogLevel level, string message)
        {
            if (level >= LogLevel.Warning)
            {
                Console.WriteLine(message);
            }
        }

        private void LogToFile(LogLevel level, string message)
        {
            var formatted = GetMessageWithTimestamp(level, message);
            File.AppendAllLines(_logPath, new[]{ formatted });
        }
    }

    internal enum LogLevel
    {
        Debug,
        Warning,
        Error,
        Info,
    }
}
