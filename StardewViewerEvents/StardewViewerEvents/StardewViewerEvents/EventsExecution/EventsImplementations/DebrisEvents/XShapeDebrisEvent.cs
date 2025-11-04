using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class XShapeDebrisEvent : DebrisEvent
    {
        public XShapeDebrisEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var debrisId = _debrisSpawner.ChooseRandomDebris();
            var chosenXRadius = Game1.random.Next(1, 5);
            var playerTile = Game1.player.Tile;

            _debrisSpawner.SpawnSingleDebris(Game1.currentLocation, playerTile, debrisId);
            for (var i = 1; i < chosenXRadius; i++)
            {
                _debrisSpawner.SpawnSingleDebris(Game1.currentLocation, playerTile + new Vector2(-i, -i), debrisId);
                _debrisSpawner.SpawnSingleDebris(Game1.currentLocation, playerTile + new Vector2(i, -i), debrisId);
                _debrisSpawner.SpawnSingleDebris(Game1.currentLocation, playerTile + new Vector2(-i, i), debrisId);
                _debrisSpawner.SpawnSingleDebris(Game1.currentLocation, playerTile + new Vector2(i, i), debrisId);
            }
        }

        private static readonly Dictionary<int, List<Vector2>> _circleShapes = new()
        {
            {3, new List<Vector2>
            {
                new (-1, -1), new (0, -1), new (1, -1),
                new (-1, 0),               new (1, 0),
                new (-1, 1),  new (0, 1),  new (1, 1),
            }},
            {5, new List<Vector2>
            {
                              new (-1, -2), new(0, -2), new (1, -2),
                new (-2, -1),                                        new (2, -1),
                new (-2, 0),                                         new (2, 0),
                new (-2, 1),                                         new (2, 1),
                              new (-1, 2),  new (0, 2), new (1, 2),
            }},
            {7, new List<Vector2>
            {
                                            new (-1, -3), new(0, -3), new (1, -3), 
                              new (-2, -2),                                        new (2, -2),
                new (-3, -1),                                                                   new (3, -1),
                new (-3, -0),                                                                   new (3, 0),
                new (-3, 1),                                                                    new (3, 1),
                              new (-2, 2),                                         new (2, 2),  
                                            new (-1, 3),  new (0, 3), new (1, 3),  
            }},
        };
    }
}
