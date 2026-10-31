namespace ViewerEventsLibrary.Logging
{
    public interface IViewerEventsLogger
    {
        void LogError(string message);
        void LogError(string message, Exception exception);
        void LogWarning(string message);
        void LogDebug(string message);
        void LogInfo(string message);
    }
}
