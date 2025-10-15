using CustomMail.CustomMail;
using HarmonyLib;
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace CustomMail
{
    internal sealed class ModEntry : Mod
    {
        public static ModEntry Instance;
        private IModHelper _helper;
        private CustomMailManager _customMailManager;

        public ModEntry() : base()
        {
            Instance = this;
        }

        /*********
         ** Public methods
         *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            _helper = helper;
            _customMailManager = new CustomMailManager(Monitor, _helper);

            _helper.Events.GameLoop.DayStarted += OnDayStarted;

            _helper.ConsoleCommands.Add("add_letter", $"Adds a letter to the bottom of the mailbox. Syntax: add_letter [letter text]", OnAddLetter);
            _helper.ConsoleCommands.Add("add_letter_top", $"Adds a letter to the top of the mailbox. Syntax: add_letter_top [letter text]", OnAddLetterTop);
            _helper.ConsoleCommands.Add("add_letter_tomorrow", $"Adds a letter to the mail for tomorrow. Syntax: add_letter_tomorrow [letter text]", OnAddLetterTomorrow);
        }

        private void OnAddLetter(string arg1, string[] arg2)
        {
            var content = string.Join(" ", arg2);
            _customMailManager.AddLetterToday(content, false);
        }

        private void OnAddLetterTop(string arg1, string[] arg2)
        {
            var content = string.Join(" ", arg2);
            _customMailManager.AddLetterToday(content, true);
        }

        private void OnAddLetterTomorrow(string arg1, string[] arg2)
        {
            var content = string.Join(" ", arg2);
            _customMailManager.AddLetterTomorrow(content);
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            _customMailManager.GeneratePlaceholderContentForUnknownLetters();
        }
    }
}
