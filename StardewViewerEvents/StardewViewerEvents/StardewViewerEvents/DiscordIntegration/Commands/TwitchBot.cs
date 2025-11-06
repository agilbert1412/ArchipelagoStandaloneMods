using HarmonyLib;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Network;
using StardewViewerEvents.Credits;
using StardewViewerEvents.EventsExecution;
using StardewViewerEvents.Extensions;
using System.Net.Sockets;
using Microsoft.Xna.Framework;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace StardewViewerEvents.DiscordIntegration.Commands
{
    public class TwitchBot
    {
        private const string COMMAND_PREFIX = "!";

        private IMonitor _logger;
        private static SimplifiedCommandsHandler _simplifiedCommandsHandler;
        private static CreditAccounts _accounts;
        private static ViewerEventsExecutor _eventsExecutor;

        TwitchClient client;

        public TwitchBot(IMonitor logger, ModConfig config, CreditAccounts accounts, ViewerEventsExecutor eventsExecutor, CommandReader commandReader)
        {
            _logger = logger;
            _accounts = accounts;
            _eventsExecutor = eventsExecutor;
            _simplifiedCommandsHandler = new SimplifiedCommandsHandler(commandReader, COMMAND_PREFIX);

            var credentials = new ConnectionCredentials(config.TwitchBotUsername, config.TwitchBotToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            var customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, config.TwitchChannel);

            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnMessageReceived += Client_OnMessageReceived;
            // client.OnWhisperReceived += Client_OnWhisperReceived;
            // client.OnNewSubscriber += Client_OnNewSubscriber;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            _logger.LogDebug($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            _logger.LogInfo($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            var onJoinMessage = "Stardew Viewer Events Bot connected and ready!";
            _logger.LogInfo(onJoinMessage);
            client.SendMessage(e.Channel, onJoinMessage);
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            var message = e.ChatMessage.Message;
            var messageLower = message.ToLower();

            if (message.Equals("test", StringComparison.InvariantCultureIgnoreCase))
            {
                client.SendMessage(e.ChatMessage.Channel, "Toast");
                return;
            }

            if (string.IsNullOrWhiteSpace(message) || !message.StartsWith(COMMAND_PREFIX))
            {
                return;
            }

            var senderName = e.ChatMessage.Username;
            var author = _accounts.GetAccountFromTwitchLink(senderName);

            if (string.IsNullOrWhiteSpace(senderName) || author == null)
            {
                client.SendMessage(e.ChatMessage.Channel, $"{senderName} tried to use a command, but has no linked account active. Head over to the Discord to set up a link!");
                return;
            }
            
            if (_simplifiedCommandsHandler.HandleCreditsUserCommands(message, _accounts, author, out var response) ||
                _simplifiedCommandsHandler.HandleEventsUserCommands(message, _accounts, _eventsExecutor, author, out response))
            {
                client.SendMessage(e.ChatMessage.Channel, response);
                return;
            }

            return;
        }

        //private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        //{
        //    if (e.WhisperMessage.Username == "my_friend")
        //    {
        //        client.SendWhisper(e.WhisperMessage.Username, "Hey! Whispers are so cool!!");
        //    }
        //}

        //private void Client_OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        //{
        //    if (e.Subscriber.SubscriptionPlan == SubscriptionPlan.Prime)
        //        client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points! So kind of you to use your Twitch Prime on this channel!");
        //    else
        //        client.SendMessage(e.Channel, $"Welcome {e.Subscriber.DisplayName} to the substers! You just earned 500 points!");
        //}
    }
}