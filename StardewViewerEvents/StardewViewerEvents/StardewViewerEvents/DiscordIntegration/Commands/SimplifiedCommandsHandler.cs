using StardewViewerEvents.Credits;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution;

namespace StardewViewerEvents.DiscordIntegration.Commands
{
    internal class SimplifiedCommandsHandler
    {
        private readonly string _prefix;
        private readonly CommandReader _commandReader;

        public SimplifiedCommandsHandler(CommandReader commandReader, string prefix)
        {
            _prefix = prefix;
            _commandReader = commandReader;
        }

        #region Handle Commands

        public bool HandleCreditsAdminCommands(string messageText, CreditAccounts creditAccounts)
        {
            return HandleCreditsAdminCommands(messageText, creditAccounts, out _);
        }

        public bool HandleCreditsAdminCommands(string messageText, CreditAccounts creditAccounts, out string response)
        {
            creditAccounts.CreateBackup(5);

            return HandleReadCreditsOfSomeone(messageText, creditAccounts, out response);
        }

        public bool HandleCreditsUserCommands(string messageText, CreditAccounts creditAccounts, Author sender)
        {
            return HandleCreditsUserCommands(messageText, creditAccounts, sender, out _);
        }

        public bool HandleCreditsUserCommands(string messageText, CreditAccounts creditAccounts, Author sender, out string response)
        {
            return HandleReadCredits(messageText, creditAccounts, sender, out response);
        }

        public bool HandleEventsAdminCommands(string messageText,
            ViewerEventsExecutor eventExecutor)
        {
            return HandleEventsAdminCommands(messageText, eventExecutor, out _);
        }

        public bool HandleEventsAdminCommands(string messageText, ViewerEventsExecutor eventExecutor, out string response)
        {
            return HandleGlobalPause(messageText, eventExecutor, out response);
        }

        public bool HandleEventsUserCommands(string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, Author sender)
        {
            return HandleEventsUserCommands(messageText, creditAccounts, eventExecutor, sender, out _);
        }

        public bool HandleEventsUserCommands(string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, Author sender, out string response)
        {
            return HandleCommandPurchase(messageText, creditAccounts, eventExecutor, sender, out response) ||
                   HandleCommandPay(messageText, creditAccounts, eventExecutor, sender, out response) ||
                   HandleGetGlobalPriceMultiplier(messageText, eventExecutor, out response);
        }

#endregion Handle Commands

        private bool HandleReadCredits(string messageText, CreditAccounts creditAccounts, Author sender, out string response)
        {
            if (!messageText.Equals($"{_prefix}credits"))
            {
                response = "";
                return false;
            }

            response = TellUserHisCreditAmount(creditAccounts, sender);
            return true;
        }

        private bool HandleReadCreditsOfSomeone(string messageText, CreditAccounts creditAccounts, out string response)
        {
            if (!messageText.StartsWith($"{_prefix}credits ", StringComparison.InvariantCultureIgnoreCase))
            {
                response = "";
                return false;
            }

            if (!_commandReader.IsCommandValid(messageText, out ulong discordId))
            {
                response = $"Usage: {_prefix}credits [discordId]";
                return false;
            }

            response = TellAdminCreditAmountOfSomeone(creditAccounts, discordId);
            return true;
        }

        private string TellUserHisCreditAmount(CreditAccounts creditAccounts, Author sender)
        {
            var userAccount = creditAccounts[sender.Id];
            var creditAmount = userAccount.GetCredits();
            return $@"You currently have {creditAmount} credits.";
        }

        private string TellAdminCreditAmountOfSomeone(CreditAccounts creditAccounts, ulong discordId)
        {
            var userAccount = creditAccounts[discordId];
            var userName = userAccount.discordName;
            var creditAmount = userAccount.GetCredits();
            return $@"{userName} currently has {creditAmount} credits.";
        }

        private bool HandleGlobalPause(string messageText, ViewerEventsExecutor eventExecutor, out string response)
        {
            if (messageText.StartsWith($"{_prefix}pause", StringComparison.InvariantCultureIgnoreCase))
            {
                eventExecutor.Queue.Pause();
                response = $"All eventExecutor.Events are now paused";
                return true;
            }

            if (messageText.StartsWith($"{_prefix}unpause", StringComparison.InvariantCultureIgnoreCase) || messageText.StartsWith($"{_prefix}resume", StringComparison.InvariantCultureIgnoreCase))
            {
                eventExecutor.Queue.Unpause();
                response = $"All eventExecutor.Events are now resumed";
                return true;
            }

            response = "";
            return false;
        }

        private bool HandleGetGlobalPriceMultiplier(string messageText, ViewerEventsExecutor eventExecutor, out string response)
        {
            if (!messageText.Equals($"{_prefix}prices"))
            {
                response = "";
                return false;
            }

            response = $"The global event price is currently {eventExecutor.Events.CurrentMultiplier}";
            return true;
        }

        private bool HandleCommandPurchase(string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, Author sender, out string response)
        {
            if (!messageText.StartsWith($"{_prefix}purchase ", StringComparison.InvariantCultureIgnoreCase))
            {
                response = "";
                return false;
            }

            Console.WriteLine("Purchase: " + messageText);

            if (!_commandReader.IsCommandValid(messageText, out var eventName, out string[] parameters))
            {
                response = "Usage: !purchase [eventName]";
                return true;
            }

            var chosenEvent = eventExecutor.Events.GetEvent(eventName);
            if (chosenEvent == null)
            {
                response = $"{eventName} is not a valid event";
                return true;
            }

            var costForNextStack = chosenEvent.GetCostToNextActivation(eventExecutor.Events.CurrentMultiplier);

            LogPay(sender.DisplayName, costForNextStack);
            response = PayForEvent(creditAccounts, eventExecutor, chosenEvent, costForNextStack, sender, parameters);
            return true;
        }

        private bool HandleCommandPay(string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, Author sender, out string response)
        {
            if (!messageText.StartsWith($"{_prefix}pay ", StringComparison.InvariantCultureIgnoreCase))
            {
                response = "";
                return false;
            }

            Console.WriteLine("Pay: " + messageText);
            if (!_commandReader.IsCommandValid(messageText, out var eventName, out var creditsToPay, out var args))
            {
                response = "Usage: !pay [eventName] [creditAmount] [args]";
                return true;
            }

            LogPay(sender.DisplayName, creditsToPay);

            if (creditsToPay <= 0)
            {
                response = $"{creditsToPay} is not a valid amount of credits";
                return true;
            }

            response = PayForEvent(creditAccounts, eventExecutor, eventName, creditsToPay, sender, args);
            return true;
        }

        private string PayForEvent(CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, string eventName, int creditsToPay, Author sender, string[] args)
        {
            var chosenEvent = eventExecutor.Events.GetEvent(eventName);
            if (chosenEvent == null)
            {
                return $"{eventName} is not a valid event";
            }

            return PayForEvent(creditAccounts, eventExecutor, chosenEvent, creditsToPay, sender, args);
        }

        private string PayForEvent(CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, ViewerEvent chosenEvent, int creditsToPay, Author sender, string[] args)
        {
            var userAccount = creditAccounts[sender.Id];

            if (!chosenEvent.IsQueueable())
            {
                var costToNextActivation = chosenEvent.GetCostToNextActivation(eventExecutor.Events.CurrentMultiplier);
                if (creditsToPay > costToNextActivation)
                {
                    creditsToPay = costToNextActivation;
                }
            }

            if (creditsToPay > userAccount.GetCredits())
            {
                return $"You cannot afford to pay {creditsToPay} credits. Balance: {userAccount.GetCredits()}";
            }

            chosenEvent.AddToBank(creditsToPay);
            userAccount.RemoveCredits(creditsToPay);

            var numberOfActivations = TriggerEventAsNeeded(sender, chosenEvent, eventExecutor, args);

            eventExecutor.Queue.PrintToConsole();
            if (numberOfActivations > 0)
            {
                return $"Received {creditsToPay} credits from {sender.DisplayName} to activate {chosenEvent.name} {numberOfActivations} times!";
            }
            else
            {
                return $"Received {creditsToPay} credits from {sender.DisplayName}.  {chosenEvent.name} is now at {chosenEvent.GetBank()}/{chosenEvent.GetMultiplierCost(eventExecutor.Events.CurrentMultiplier)}.";
            }

        }

        private int TriggerEventAsNeeded(Author sender, ViewerEvent chosenEvent, ViewerEventsExecutor eventExecutor, string[] args)
        {
            var numberOfActivations = 0;
            while (chosenEvent.GetBank() >= chosenEvent.GetMultiplierCost(eventExecutor.Events.CurrentMultiplier))
            {
                chosenEvent.CallEvent(eventExecutor.Events.CurrentMultiplier);
                LogEvent(sender.Username, chosenEvent);
                eventExecutor.AddOrIncrementEventInQueue(sender, chosenEvent, args);
                numberOfActivations++;
            }

            return numberOfActivations;
        }

        public void LogEvent(string senderName, ViewerEvent calledEvent)
        {
            var localDate = DateTime.Now;
            using var w = File.AppendText("EventLog.txt");
            w.WriteLine($"[{localDate}] {senderName} activated {calledEvent.name}.");
        }

        public void LogPay(string senderName, int payAmount)
        {
            var localDate = DateTime.Now;
            using var w = File.AppendText("PayLog.txt");
            w.WriteLine($"[{localDate}] {senderName} paid {payAmount} credits.");
        }
    }
}