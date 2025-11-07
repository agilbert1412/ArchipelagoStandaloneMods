using Discord;
using Discord.WebSocket;
using StardewViewerEvents.Credits;
using StardewViewerEvents.Events;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.Extensions;

namespace StardewViewerEvents.DiscordIntegration.Commands
{
    public class EventsCommandsHandler
    {
        private readonly IBotCommunicator _communications;
        private readonly CommandReader _commandReader;
        private readonly HelpProvider _helpProvider;

        public EventsCommandsHandler(IBotCommunicator discord, CommandReader commandReader, HelpProvider helpProvider)
        {
            _communications = discord;
            _commandReader = commandReader;
            _helpProvider = helpProvider;
        }

        public void HandleEventsAdminCommands(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            HandleQueueEvent(message, messageText, eventExecutor);
            HandleTriggerEvent(message, messageText, eventExecutor);
            HandleSetBank(message, messageText, eventExecutor);
            HandleSetGlobalPriceMultiplier(message, messageText, eventExecutor);
            HandleGlobalPause(message, messageText, eventExecutor);
            HandleTestAllEvents(message, messageText, eventExecutor);
        }

        public async Task HandleEventsUserCommands(SocketUserMessage message, string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor)
        {
            HandleCommandBank(message, messageText, eventExecutor);
            await HandleCommandPurchase(message, messageText, creditAccounts, eventExecutor);
            await HandleCommandPay(message, messageText, creditAccounts, eventExecutor);
            HandleGetGlobalPriceMultiplier(message, messageText, eventExecutor);
        }

        private void HandleQueueEvent(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!queueevent ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (!_commandReader.IsCommandValid(messageText, out var eventName, out string[] args))
            {
                _communications.ReplyTo(message, "Usage: !queueevent [eventName]");
                return;
            }

            var chosenEvent = eventExecutor.Events.GetEvent(eventName);

            if (chosenEvent == null)
            {
                _communications.ReplyTo(message, $"{eventName} is not a valid event");
                return;
            }

            eventExecutor.AddOrIncrementEventInQueue(message.Author, chosenEvent, args);
            _communications.ReplyTo(message, $"Queued up one instance of {chosenEvent.name}.");
            eventExecutor.Queue.PrintToConsole();
        }

        private void HandleTriggerEvent(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!triggerevent ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (!_commandReader.IsCommandValid(messageText, out var eventName, out string[] args))
            {
                _communications.ReplyTo(message, "Usage: !triggerevent [eventName]");
                return;
            }

            var chosenEvent = eventExecutor.Events.GetEvent(eventName);

            if (chosenEvent == null)
            {
                _communications.ReplyTo(message, $"{eventName} is not a valid event");
                return;
            }

            var forcedEvent = new ViewerEvent();
            forcedEvent.name = eventName;
            forcedEvent.SetBank(1);
            forcedEvent.SetCost(1);
            var queuedForcedEvent = new QueuedEvent(forcedEvent, args);

            queuedForcedEvent.queueCount = 1;
            queuedForcedEvent.username = message.Author.GetDisplayName();
            queuedForcedEvent.userId = message.Author.Id;
            eventExecutor.Queue.PushAtBeginning(queuedForcedEvent);
            _communications.ReplyTo(message, $"Forced {forcedEvent.name} immediately.");

            eventExecutor.Queue.PrintToConsole();
        }

        private void HandleSetBank(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!setbank ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (!_commandReader.IsCommandValid(messageText, out string eventName, out int bankAmount, out _))
            {
                _communications.ReplyTo(message, "Usage: !setbank [eventName] [bankAmount]");
                return;
            }

            if (bankAmount < 0)
            {
                _communications.ReplyTo(message, $"{bankAmount} is not a valid amount of credits");
                return;
            }

            eventName = eventName.ToLower().Replace(" ", "");

            foreach (var e in eventExecutor.Events.ToList())
            {
                if (e.name.ToLower().Replace(" ", "") == eventName)
                {
                    e.SetBank(bankAmount);
                    _communications.ReplyTo(message, $"{e.name} set to {e.GetBank()} credits.");
                }
            }
        }

        private void HandleSetGlobalPriceMultiplier(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!setmultiplier ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (!_commandReader.IsCommandValid(messageText, out double multiplier))
            {
                _communications.ReplyTo(message, "Usage: !setmultiplier [multiplier]");
                return;
            }

            eventExecutor.Events.CurrentMultiplier = multiplier;
            _communications.SetStatusMessage($"your donations. Price Multiplier: {eventExecutor.Events.CurrentMultiplier}", ActivityType.Listening);
            _communications.ReplyTo(message, $"Set global event price multiplier to {multiplier}");
            _helpProvider.SendAllEventsHelpMessages(eventExecutor.Events);
        }

        private void HandleGlobalPause(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (messageText.StartsWith("!pause", StringComparison.InvariantCultureIgnoreCase))
            {
                eventExecutor.Queue.Pause();
                _communications.ReplyTo(message, $"All eventExecutor.Events are now paused");
                return;
            }

            if (messageText.StartsWith("!unpause", StringComparison.InvariantCultureIgnoreCase) || messageText.StartsWith("!resume", StringComparison.InvariantCultureIgnoreCase))
            {
                eventExecutor.Queue.Unpause();
                _communications.ReplyTo(message, $"All eventExecutor.Events are now resumed");
                return;
            }
        }

        private void HandleTestAllEvents(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!testallevents", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var counter = 0;
            var random = new Random();
            foreach (var (eventName, viewerEvent) in eventExecutor.Events._events.OrderBy(x => random.NextDouble()))
            {
                eventExecutor.AddEventToQueue(message.Author, viewerEvent, viewerEvent.hasParameters ? new []{"test"} : Array.Empty<string>());
                counter++;
            }

            _communications.ReplyTo(message, $"Queued up one instance of all {counter} available events.");
            eventExecutor.Queue.PrintToConsole();
        }

        private void HandleGetGlobalPriceMultiplier(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.Equals("!prices"))
            {
                return;
            }

            _communications.ReplyTo(message, $"The global event price is currently {eventExecutor.Events.CurrentMultiplier}");
        }

        private void HandleCommandBank(SocketUserMessage message, string messageText, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!bank ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            if (!_commandReader.IsCommandValid(messageText, out string eventName))
            {
                _communications.ReplyTo(message, "Usage: !bank [eventName]");
                return;
            }

            CheckEventBank(message, eventExecutor, eventName);
        }

        private async Task HandleCommandPurchase(SocketUserMessage message, string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!purchase ", StringComparison.InvariantCultureIgnoreCase) && !messageText.StartsWith("!buy ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            Console.WriteLine("Purchase: " + messageText);

            if (!_commandReader.IsCommandValid(messageText, out var eventName, out string[] parameters))
            {
                _communications.ReplyTo(message, "Usage: !purchase [eventName]");
                return;
            }

            var chosenEvent = eventExecutor.Events.GetEvent(eventName);
            if (chosenEvent == null)
            {
                _communications.ReplyTo(message, $"{eventName} is not a valid event");
                return;
            }

            var costForNextStack = chosenEvent.GetCostToNextActivation(eventExecutor.Events.CurrentMultiplier);

            LogPay(message.Author.Username, costForNextStack);
            await PayForEvent(message, creditAccounts, eventExecutor, chosenEvent, costForNextStack, parameters);
        }

        private async Task HandleCommandPay(SocketUserMessage message, string messageText, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor)
        {
            if (!messageText.StartsWith("!pay ", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            Console.WriteLine("Pay: " + messageText);
            if (!_commandReader.IsCommandValid(messageText, out var eventName, out var creditsToPay, out var args))
            {
                _communications.ReplyTo(message, "Usage: !pay [eventName] [creditAmount] [args]");
                return;
            }

            LogPay(message.Author.Username, creditsToPay);

            if (creditsToPay <= 0)
            {
                _communications.ReplyTo(message, $"{creditsToPay} is not a valid amount of credits");
                return;
            }

            await PayForEvent(message, creditAccounts, eventExecutor, eventName, creditsToPay, args);
        }

        private async Task PayForEvent(SocketUserMessage message, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, string eventName, int creditsToPay, string[] args)
        {
            var chosenEvent = eventExecutor.Events.GetEvent(eventName);
            if (chosenEvent == null)
            {
                _communications.ReplyTo(message, $"{eventName} is not a valid event");
                return;
            }

            await PayForEvent(message, creditAccounts, eventExecutor, chosenEvent, creditsToPay, args);
        }

        private async Task PayForEvent(SocketUserMessage message, CreditAccounts creditAccounts, ViewerEventsExecutor eventExecutor, ViewerEvent chosenEvent, int creditsToPay, string[] args)
        {
            var userAccount = creditAccounts[message.Author.Id];

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
                _communications.ReplyTo(message, $"You cannot afford to pay {creditsToPay} credits. Balance: {userAccount.GetCredits()}");
                return;
            }

            chosenEvent.AddToBank(creditsToPay);
            userAccount.RemoveCredits(creditsToPay);

            var numberOfActivations = TriggerEventAsNeeded(message.Author, chosenEvent, eventExecutor, args);

            if (numberOfActivations > 0)
            {
                _communications.ReplyTo(message,
                    $"Received {creditsToPay} credits from {message.Author.Username} to activate {chosenEvent.name} {numberOfActivations} times!");
            }
            else
            {
                _communications.ReplyTo(message,
                    $"Received {creditsToPay} credits from {message.Author.Username}.  {chosenEvent.name} is now at {chosenEvent.GetBank()}/{chosenEvent.GetMultiplierCost(eventExecutor.Events.CurrentMultiplier)}.");
            }

            eventExecutor.Queue.PrintToConsole();
        }

        private int TriggerEventAsNeeded(SocketUser sender, ViewerEvent chosenEvent, ViewerEventsExecutor eventExecutor, string[] args)
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

        private void CheckEventBank(SocketUserMessage message, ViewerEventsExecutor eventExecutor, string eventName)
        {
            var invokedEvent = eventExecutor.Events.GetEvent(eventName);
            if (invokedEvent == null)
            {
                _communications.ReplyTo(message, $"{eventName} is not a valid Event.");
                return;
            }

            _communications.ReplyTo(message, $"{invokedEvent.name} is at {invokedEvent.GetBank()}/{invokedEvent.GetMultiplierCost(eventExecutor.Events.CurrentMultiplier)} credits.");
        }
    }
}