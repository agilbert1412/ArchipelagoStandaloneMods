using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewViewerEvents.Events;

namespace StardewViewerEvents.EventsExecution.EventsImplementations.DebrisEvents
{
    public class OShapeDebrisEvent : DebrisEvent
    {
        public OShapeDebrisEvent(IMonitor logger, IModHelper modHelper, QueuedEvent queuedEvent) : base(logger, modHelper, queuedEvent)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var debrisId = _debrisSpawner.ChooseRandomDebris();
            var circleSizes = _circleShapes.Keys.ToArray();
            var chosenCircleSize = circleSizes[Game1.random.Next(circleSizes.Length)];
            var chosenCircleOffsets = _circleShapes[chosenCircleSize];

            foreach (var offset in chosenCircleOffsets)
            {
                var debrisTile = Game1.player.Tile + offset;
                _debrisSpawner.SpawnSingleDebris(Game1.currentLocation, debrisTile, debrisId);
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
            {9, new List<Vector2>
            {
                                            new (-2, -4), new (-1, -4), new(0, -4), new (1, -4), new (2, -4),
                              new (-3, -3),                                                                   new (3, -3),
                new (-4, -2),                                                                                              new (4, -2),
                new (-4, -1),                                                                                              new (4, -1),
                new (-4, 0),                                                                                               new (4, 0),
                new (-4, 1),                                                                                               new (4, 1),
                new (-4, 2),                                                                                               new (4, 2),
                              new (-3, 3),                                                                    new (3, 3),
                                            new (-2, 4),  new (-1, 4),  new(0, 4),  new (1, 4),  new (2, 4),
            }},
            {11, new List<Vector2>
            {
                                                          new (-2, -5), new (-1, -5), new(0, -5), new (1, -5), new (2, -5),
                                            new (-3, -4),                                                                   new (3, -4),
                              new (-4, -3),                                                                                              new (4, -3),
                new (-5, -2),                                                                                                                         new (5, -2),
                new (-5, -1),                                                                                                                         new (5, -1),
                new (-5, 0),                                                                                                                          new (5, 0),
                new (-5, 1),                                                                                                                          new (5, 1),
                new (-5, 2),                                                                                                                          new (5, 2),
                              new (-4, 3),                                                                                               new (4, 3),
                                            new (-3, 4),                                                                    new (3, 4),
                                                          new (-2, 5),  new (-1, 5),  new(0, 5),  new (1, 5),  new (2, 5),
            }},
        };
    }
}
