using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
