using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewViewerEvents.Events;
using System.Text;

namespace StardewViewerEvents.DiscordIntegration
{
    public class ChannelSet
    {
        private const string ERROR_NO_CHANNELS = "Channels are not set! Make sure you set up your Discord Channels in `/PersistentData/ChannelSet.json`.";
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
                throw new Exception(ERROR_NO_CHANNELS);
            }

            var lines = File.ReadAllText(channelsFile, Encoding.UTF8);
            dynamic jsonData = JsonConvert.DeserializeObject(lines);
            DebugChannel = ulong.Parse(jsonData["DebugChannel"].ToString());
            DonationsChannel = ulong.Parse(jsonData["DonationsChannel"].ToString());
            EventsChannel = ulong.Parse(jsonData["EventsChannel"].ToString());
            ChatChannel = ulong.Parse(jsonData["ChatChannel"].ToString());
            HelpCommandsChannel = ulong.Parse(jsonData["HelpCommandsChannel"].ToString());
            HelpEventsChannel = ulong.Parse(jsonData["HelpEventsChannel"].ToString());
            AdminHelpChannel = ulong.Parse(jsonData["AdminHelpChannel"].ToString());
            AdminChannel = ulong.Parse(jsonData["AdminChannel"].ToString());
            AdminPing = jsonData["AdminPing"].ToString();

            ValidateChannels();
        }

        public void ExportTo(string channelsFile)
        {
            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(channelsFile, json);
        }

        private void ValidateChannels()
        {
            var errorChannels = new List<string>();
            ValidateChannel(errorChannels, nameof(DebugChannel), DebugChannel);
            ValidateChannel(errorChannels, nameof(DonationsChannel), DonationsChannel);
            ValidateChannel(errorChannels, nameof(EventsChannel), EventsChannel);
            ValidateChannel(errorChannels, nameof(ChatChannel), ChatChannel);
            ValidateChannel(errorChannels, nameof(HelpCommandsChannel), HelpCommandsChannel);
            ValidateChannel(errorChannels, nameof(HelpEventsChannel), HelpEventsChannel);
            ValidateChannel(errorChannels, nameof(AdminHelpChannel), AdminHelpChannel);
            ValidateChannel(errorChannels, nameof(AdminChannel), AdminChannel);
            if (errorChannels.Any())
            {
                throw new Exception($"{ERROR_NO_CHANNELS} Channels in error: [{string.Join(", ", errorChannels)}]");
            }
        }

        private void ValidateChannel(List<string> errorChannels, string channelName, ulong channelValue)
        {
            if (channelValue <= 0)
            {
                errorChannels.Add(channelName);
            }
        }
    }
}
