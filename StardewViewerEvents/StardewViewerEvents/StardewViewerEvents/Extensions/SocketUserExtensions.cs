using Discord.WebSocket;

namespace StardewViewerEvents.Extensions
{
    public static class SocketUserExtensions
    { 
        public static string GetDisplayName(this SocketUser user)
        {
            if (!string.IsNullOrWhiteSpace(user.GlobalName))
            {
                return user.GlobalName;
            }

            return user.Username;
        }
    }
}
