namespace StardewViewerEvents
{
    public class ModConfig
    {
        /// <summary>
        /// Delay in seconds between events triggering
        /// </summary>
        public uint EventsDelay { get; set; } = 5;

        /// <summary>
        /// Token to log in to Discord
        /// </summary>
        public string DiscordToken { get; set; } = "";

        /// <summary>
        /// Your own Discord ID, allowing you to use your credits account from in-game chat
        /// </summary>
        public string DiscordId { get; set; } = "";

        /// <summary>
        /// Username of the Twitch Bot
        /// </summary>
        public string TwitchBotUsername { get; set; } = "";

        /// <summary>
        /// Token to log in to Twitch
        /// </summary>
        public string TwitchBotToken { get; set; } = "";

        /// <summary>
        /// Twitch Channel to join
        /// </summary>
        public string TwitchChannel { get; set; } = "";
    }
}
