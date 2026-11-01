using RealLifeViewerEvents.Events;
using RealLifeViewerEvents.Events.Constants;
using RealLifeViewerEvents.Extensions;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution.EventsImplementations
{
    public class KaraokeSongEvent : ExecutableEvent
    {
        private bool _isStarted;
        private bool _isDone;

        public KaraokeSongEvent(IViewerEventsLogger logger, QueuedEvent queuedEvent) : base(logger, queuedEvent)
        {
            _isStarted = false;
            _isDone = false;
        }

        public override bool CanExecuteRightNow()
        {
            return base.CanExecuteRightNow();
        }

        public override void Execute()
        {
            base.Execute();
            _isStarted = true;
            Console.WriteLine($"Type 'd' when done");
        }

        public override void ReceivedInput(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            if (input.ToLower().StartsWith("d"))
            {
                _isDone = true;
            }
        }

        public override bool UpdateAndTryFinish(int deltaMilliseconds)
        {
            return _isDone;
        }

        public override bool ValidateParameters(out string errorMessage)
        {
            if (QueuedEvent.parameters == null || QueuedEvent.parameters.Length < 1)
            {
                errorMessage = "You must specify a song from the list";
                foreach (var song in KaraokeSongs.SONG_TITLES)
                {
                    errorMessage += $"{Environment.NewLine}\t - {song}";
                }
                return false;
            }

            var desiredSongName = GetSingleParameter();
            if (!TryGetDesiredSong(desiredSongName, out var correctSongName))
            {
                errorMessage = $"Unrecognized song name [{desiredSongName}]. You must specify a song from the following list:";
                foreach (var song in KaraokeSongs.SONG_TITLES)
                {
                    errorMessage += $"{Environment.NewLine}\t - {song}";
                }
                return false;
            }

            errorMessage = "";
            return true;
        }

        private bool TryGetDesiredSong(string desiredSongName, out string correctSongName)
        {
            correctSongName = string.Empty;
            if (string.IsNullOrEmpty(desiredSongName))
            {
                return false;
            }

            var sanitizedDesiredSongName = desiredSongName.SanitizeName();
            foreach (var song in KaraokeSongs.SONG_TITLES)
            {
                var sanitizedTitle = song.SanitizeName();
                if (sanitizedTitle == sanitizedDesiredSongName)
                {
                    correctSongName = song;
                    return true;
                }
            }

            return false;
        }
    }
}
