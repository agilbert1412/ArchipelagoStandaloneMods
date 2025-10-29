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
        /// Token to log in to Twitch
        /// </summary>
        public string TwitchToken { get; set; } = "";
    }
}
