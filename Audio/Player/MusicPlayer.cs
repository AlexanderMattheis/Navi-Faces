using System;
using System.Collections.Generic;
using System.Threading;

namespace Navi.Audio.Player
{
    public sealed class MusicPlayer : AudioPlayer, IDisposable
    {
        private const int MillisecondsToStopCheck = 1000;

        private int currentTitle;

        private MusicData musicFile;

        private Thread stopCheck;

        public override void Init(int sampleRate = 44100, int channels = 2)
        {
            base.Init(sampleRate, channels);
            StartStopCheck();
        }

        private void StartStopCheck()
        {
            stopCheck = new Thread(new ThreadStart(Stopped));
            stopCheck.Start();
        }

        private void Stopped()
        {
            while (true)
            {
                if (musicFile.Finished)
                {
                    musicFile.Finished = false;
                    PlayNextOfSameType();
                }

                Thread.Sleep(MillisecondsToStopCheck);
            }             
        }

        private void PlayNextOfSameType()
        {
            string nextFilePath = string.Empty;

            switch (musicFile.MusicType)
            {
                case Music.Types.MainMenu: nextFilePath = NextSongFromList(Music.MainMenu); break;
                case Music.Types.Ingame: nextFilePath = NextSongFromList(Music.Ingame); break;
            }

            if (nextFilePath != string.Empty)
            {
                MusicData song = new MusicData(nextFilePath, musicFile.MusicType);
                Play(song);
            }
        }

        private string NextSongFromList(List<string> songsList)
        {
            currentTitle++;

            if (songsList.Count <= currentTitle) currentTitle = 0;

            return songsList[currentTitle];
        }

        public void Play(MusicData musicFile)
        {
            this.musicFile = musicFile;
            base.Play(musicFile);
        }

        public override void Dispose()
        {
            stopCheck.Abort();
            base.Dispose();
        }
    }
}
