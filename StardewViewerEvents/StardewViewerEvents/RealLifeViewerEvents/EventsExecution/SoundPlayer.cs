using NAudio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using ViewerEventsLibrary.Extensions;
using ViewerEventsLibrary.Logging;

namespace RealLifeViewerEvents.EventsExecution
{
    public class SoundPlayer
    {
        private const string SOUNDS_PATH = "Sounds";
        private const string NEW_EVENT_FILE = "new_event.wav";
        private const string EVENT_DONE_FILE = "event_done.wav";

        private IViewerEventsLogger _logger;

        public SoundPlayer(IViewerEventsLogger logger)
        {
            _logger = logger;
        }

        public void PlayNewEventSound()
        {
            var path = Path.Combine(SOUNDS_PATH, NEW_EVENT_FILE);
            PlaySound(path);
        }

        public void PlayEventDoneSound()
        {
            var path = Path.Combine(SOUNDS_PATH, EVENT_DONE_FILE);
            PlaySound(path);
        }

        public void PlaySound(string path)
        {
            PlaySoundAsync(path).FireAndForget();
        }

        public async Task PlaySoundAsync(string path)
        {
            try
            {
                await using var audioFile = new AudioFileReader(path);
                using var player = new DirectSoundOut();
                player.Init(audioFile);
                player.Play();
                while (player.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed at playing sound '{path}'{Environment.NewLine}\t{ex.Message}{Environment.NewLine}\t{ex.StackTrace}");
            }
        }
    }
}
