#nullable enable
namespace RealLifeViewerEvents.Extensions
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
