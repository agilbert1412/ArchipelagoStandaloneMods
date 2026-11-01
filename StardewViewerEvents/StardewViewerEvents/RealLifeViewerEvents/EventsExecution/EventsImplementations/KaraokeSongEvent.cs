using RealLifeViewerEvents.Events;
using RealLifeViewerEvents.Events.Constants;
using RealLifeViewerEvents.Extensions;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class KaraokeSongEvent : ManualEndEvent
    {

        public KaraokeSongEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (QueuedEvent.parameters == null || QueuedEvent.parameters.Length < 1)
            {
                errorMessage = "You must specify a song from the list:";
                AddAllSongsToMessage(ref errorMessage);
                return false;
            }

            var desiredSongName = GetSingleParameter();
            if (!TryGetDesiredSong(desiredSongName, out var correctSong))
            {
                errorMessage = $"Unrecognized song name [{desiredSongName}]. You must specify a song from the following list:";
                AddAllSongsToMessage(ref errorMessage);
                return false;
            }

            errorMessage = "";
            return true;
        }

        private static void AddAllSongsToMessage(ref string errorMessage)
        {
            foreach (var song in KaraokeSongs.ALL_SONGS)
            {
                errorMessage += $"{Environment.NewLine}\t - {song.Title} ({song.Context})";
            }
        }

        private bool TryGetDesiredSong(string desiredSongName, out KaraokeSong correctSong)
        {
            correctSong = null;
            if (string.IsNullOrEmpty(desiredSongName))
            {
                return false;
            }

            var sanitizedDesiredSongName = desiredSongName.SanitizeName();
            foreach (var song in KaraokeSongs.ALL_SONGS)
            {
                var sanitizedTitle = song.Title.SanitizeName();
                var sanitizedTitleWithContext = $"{sanitizedTitle} ({song.Context.SanitizeName()})";
                if (sanitizedTitle == sanitizedDesiredSongName || sanitizedTitleWithContext == sanitizedDesiredSongName)
                {
                    correctSong = song;
                    return true;
                }
            }

            return false;
        }
    }
}
