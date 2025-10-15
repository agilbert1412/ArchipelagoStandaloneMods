using HarmonyLib;
using StardewModdingAPI;
using StardewValley;

namespace CustomMail.CustomMail
{
    public class CustomMailManager
    {
        private const string CUSTOM_MAIL_KEY_PREFIX = "CustomMail_";
        private static IMonitor _logger;
        private static IModHelper _modHelper;

        public CustomMailManager(IMonitor logger, IModHelper modHelper)
        {
            _logger = logger;
            _modHelper = modHelper;
        }

        public void AddLetterToday(string letterContent, bool topOfMailbox = false)
        {
            var mailKey = GenerateLetterData(letterContent);
            if (topOfMailbox)
            {
                Game1.player.mailbox.Insert(0, mailKey);
                _logger.Log($"Added letter '{mailKey}' to top of mailbox", LogLevel.Info);
            }
            else
            {
                Game1.player.mailbox.Add(mailKey);
                _logger.Log($"Added letter '{mailKey}' to bottom of mailbox", LogLevel.Info);
            }
        }

        public void AddLetterTomorrow(string letterContent)
        {
            var mailKey = GenerateLetterData(letterContent);
            Game1.player.mailForTomorrow.Add(mailKey);
            _logger.Log($"Added letter '{mailKey}' to tomorrow's mail", LogLevel.Info);
        }

        private static string GenerateLetterData(string letterContent)
        {
            var mailData = DataLoader.Mail(Game1.content);
            var uniqueId = Guid.NewGuid().ToString();
            var mailKey = $"{CUSTOM_MAIL_KEY_PREFIX}{uniqueId}";
            mailData.Add(mailKey, letterContent);
            _logger.Log($"Generated a new letter: '{mailKey}' -> '{letterContent}'", LogLevel.Info);
            return mailKey;
        }

        public void GeneratePlaceholderContentForUnknownLetters()
        {
            const string placeholderTextTemplate = "This letter was added to the mailbox in a previous session and it doesn't have any content registered in the game data.";

            var mailData = DataLoader.Mail(Game1.content);
            foreach (var mailKey in Game1.player.mailbox.Union(Game1.player.mailForTomorrow))
            {
                if (mailData.ContainsKey(mailKey))
                {
                    continue;
                }

                mailData.Add(mailKey, placeholderTextTemplate);
                _logger.Log($"Generated a default letter for a missing id: {mailKey}", LogLevel.Debug);
            }
        }
    }
}
