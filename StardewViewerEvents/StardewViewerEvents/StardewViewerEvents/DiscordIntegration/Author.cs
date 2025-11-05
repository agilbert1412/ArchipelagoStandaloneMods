using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StardewViewerEvents.DiscordIntegration
{
    public class Author
    {
        public ulong Id { get; private set; }
        public string Username { get; private set; }
        public string DisplayName { get; private set; }

        public Author(ulong id, string username, string displayName = null)
        {
            Id = id;
            Username = username;
            DisplayName = displayName ?? username;
        }
    }
}
