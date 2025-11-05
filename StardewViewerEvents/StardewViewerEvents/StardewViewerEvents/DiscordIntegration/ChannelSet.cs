using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewViewerEvents.Events;
using System.Text;

namespace StardewViewerEvents.DiscordIntegration
{
    public class ChannelSet
    {
        public ulong DebugChannel { get; set; }
        public ulong DonationsChannel { get; set; }
        public ulong EventsChannel { get; set; }
        public ulong ChatChannel { get; set; }
        public ulong HelpCommandsChannel { get; set; }
        public ulong HelpEventsChannel { get; set; }
        public ulong AdminHelpChannel { get; set; }
        public ulong AdminChannel { get; set; }
        public string AdminPing { get; set; }

        public void ImportFrom(string channelsFile)
        {
            if (!File.Exists(channelsFile))
            {
                ExportTo(channelsFile);
                return;
            }

            var lines = File.ReadAllText(channelsFile, Encoding.UTF8);
            dynamic jsonData = JsonConvert.DeserializeObject(lines);
            DebugChannel = ulong.Parse(jsonData["DebugChannel"]);
            DonationsChannel = ulong.Parse(jsonData["DonationsChannel"]);
            EventsChannel = ulong.Parse(jsonData["EventsChannel"]);
            ChatChannel = ulong.Parse(jsonData["ChatChannel"]);
            HelpCommandsChannel = ulong.Parse(jsonData["HelpCommandsChannel"]);
            HelpEventsChannel = ulong.Parse(jsonData["HelpEventsChannel"]);
            AdminHelpChannel = ulong.Parse(jsonData["AdminHelpChannel"]);
            AdminChannel = ulong.Parse(jsonData["AdminChannel"]);
            AdminPing = jsonData["DebugChannel"].ToString();
        }

        public void ExportTo(string channelsFile)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(channelsFile, json);
        }
    }
}
