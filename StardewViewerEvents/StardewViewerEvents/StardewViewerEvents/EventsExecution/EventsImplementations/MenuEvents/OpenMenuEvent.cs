using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.MenuEvents
{
    public class OpenMenuEvent : ExecutableEvent
    {

        public OpenMenuEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var chosenMenuType = Game1.random.Next(11);

            Game1.PushUIMode();
            switch (chosenMenuType)
            {
                case 0:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.inventoryTab);
                    break;
                case 1:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.skillsTab);
                    break;
                case 2:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.socialTab);
                    break;
                case 3:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.mapTab);
                    break;
                case 4:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.craftingTab);
                    break;
                case 5:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.collectionsTab);
                    break;
                case 6:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.optionsTab);
                    break;
                case 7:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.animalsTab);
                    break;
                case 8:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.powersTab);
                    break;
                case 9:
                    Game1.activeClickableMenu = new GameMenu(GameMenu.exitTab);
                    break;
                case 10:
                    Game1.activeClickableMenu = new QuestLog();
                    break;
            }
            Game1.PopUIMode();
        }
    }
}
