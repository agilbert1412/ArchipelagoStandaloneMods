#nullable enable
namespace StardewViewerEvents.Extensions
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
