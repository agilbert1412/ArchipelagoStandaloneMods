using RealLifeViewerEvents.Events.Constants;

namespace RealLifeViewerEvents.Events
{
    public static class EventCategory
    {
        public const string SABOTAGE_GAMEPLAY = "Sabotage Gameplay Events";
        public const string WORDS = "Make me say something Events";
        public const string TALK_STYLE = "Talk Style Events";
        public const string FRIENDSHIP = "Friendship Events";
        public const string HEALTH = "Health Events";

        public static readonly Dictionary<string, string[]> CATEGORY_MAP = new()
        {
            {
                SABOTAGE_GAMEPLAY,
                new[]
                {
                    EventName.OVEN_MITTS, EventName.HORNET_MASK, EventName.BLINDFOLD, EventName.SILENCE, EventName.MUTE,
                    EventName.NO_KEYBOARD, EventName.NO_MOUSE, EventName.INVERT_HANDS,
                }
            },
            {
                WORDS,
                new[]
                {
                    EventName.KARAOKE_SONG, EventName.MOVIE_RECAP,
                }
            },
            {
                TALK_STYLE,
                new[]
                {
                    EventName.WORD_BAN, EventName.PIRATE_SPEAK,
                    EventName.NYAN_ACCENT, EventName.OPTIMIST, EventName.PESSIMIST, EventName.HYPE,
                }
            },
            {
                FRIENDSHIP,
                new[]
                {
                    EventName.DOGGO, EventName.CALL_A_FRIEND,
                }
            },
            {
                HEALTH,
                new[]
                {
                    EventName.PUSH_UPS, EventName.HYDRATE,
                }
            },
        };
    }
}
