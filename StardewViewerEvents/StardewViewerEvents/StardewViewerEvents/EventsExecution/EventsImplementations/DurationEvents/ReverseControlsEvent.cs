using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Force.DeepCloner;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DurationEvents
{
    public class ReverseControlsEvent : DurationEvent
    {
        public const int DURATION_SECONDS = 30;
        public const int DURATION_TICKS = DURATION_SECONDS * 60;

        private int _ticksDuration;
        protected override int TicksDuration => _ticksDuration;
        protected override int SecondsDuration => _ticksDuration * 60;

        private Options _originalBindings;

        public ReverseControlsEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            _ticksDuration = DURATION_TICKS * QueuedEvent.queueCount;

            _originalBindings = Game1.options.DeepClone();
            Game1.options = InvertBindings(Game1.options);
        }

        public override bool UpdateAndTryFinish()
        {
            var elapsedTicks = Game1.ticks - _tickStarted;
            var shouldEnd = elapsedTicks > TicksDuration;
            if (shouldEnd)
            {
                Game1.options = _originalBindings.DeepClone();
                return true;
            }

            return false;
        }

        private Options InvertBindings(Options options)
        {
            var invertedOptions = options.DeepClone();

            invertedOptions.actionButton = options.cancelButton;
            invertedOptions.cancelButton = options.actionButton;

            invertedOptions.chatButton = options.emoteButton;
            invertedOptions.emoteButton = options.chatButton;

            invertedOptions.journalButton = options.mapButton;
            invertedOptions.mapButton = options.journalButton;

            invertedOptions.moveDownButton = options.moveUpButton;
            invertedOptions.moveUpButton = options.moveDownButton;

            invertedOptions.moveLeftButton = options.moveRightButton;
            invertedOptions.moveRightButton = options.moveLeftButton;

            invertedOptions.runButton = options.useToolButton;
            invertedOptions.useToolButton = options.runButton;

            invertedOptions.zoomButtons = !options.zoomButtons;
            invertedOptions.alwaysShowToolHitLocation = !options.alwaysShowToolHitLocation;
            invertedOptions.autoRun = !options.autoRun;
            invertedOptions.hideToolHitLocationWhenInMotion = !options.hideToolHitLocationWhenInMotion;

            invertedOptions.inventorySlot1 = options.inventorySlot12;
            invertedOptions.inventorySlot2 = options.inventorySlot11;
            invertedOptions.inventorySlot3 = options.inventorySlot10;
            invertedOptions.inventorySlot4 = options.inventorySlot9;
            invertedOptions.inventorySlot5 = options.inventorySlot8;
            invertedOptions.inventorySlot6 = options.inventorySlot7;
            invertedOptions.inventorySlot7 = options.inventorySlot6;
            invertedOptions.inventorySlot8 = options.inventorySlot5;
            invertedOptions.inventorySlot9 = options.inventorySlot4;
            invertedOptions.inventorySlot10 = options.inventorySlot3;
            invertedOptions.inventorySlot11 = options.inventorySlot2;
            invertedOptions.inventorySlot12 = options.inventorySlot1;

            return invertedOptions;
        }
    }
}
