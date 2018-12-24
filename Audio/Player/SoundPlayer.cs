using Navi.Helper.Structures;
using System.Collections.Generic;

namespace Navi.Audio.Player
{
    public sealed class SoundPlayer : AudioPlayer
    {
        public SoundPlayer(int startCacheSize = 20)
        {
            SoundCacheSize = startCacheSize;
        }

        public int SoundCacheSize { get; private set; }

        public Dictionary<string, SoundData> Sounds { get; set; }

        public override void Init(int sampleRate = 44100, int channels = 2)
        {
            Sounds = new Dictionary<string, SoundData>(SoundCacheSize);
            base.Init(sampleRate, channels);
        }

        public void Play(string filePath, int percentVolume = 100)
        {
            SoundData sound = Sound(filePath, percentVolume);
            base.Play(sound);
        }

        private SoundData Sound(string filePath, int percentVolume)
        {
            SoundData sound;

            if (!Sounds.ContainsKey(filePath))
            {
                sound = new SoundData(filePath, percentVolume);
                Sounds.Add(filePath, sound);
            }
            else
            {
                sound = Sounds.GetValue(filePath);
                sound.ResetSamplePosition();
            } 

            return sound;
        }

        public void EmptySoundCache()
        {
            Sounds.Clear();
        }
    }
}
