using Navi.Defaults;
using System;

namespace Navi.Audio.Player
{
    public sealed class MusicData : AudioData
    {
        public MusicData(string filePath, string musicType, int percentVolume = 100)
        {
            CreateData(filePath, FileExtensions.Music, musicType, percentVolume);
        }

        public MusicData(string filePath, string fileExtension, string musicType, int percentVolume = 100)
        {
            CreateData(filePath, fileExtension, musicType, percentVolume);
        }

        public string MusicType { get; set; }

        private void CreateData(string filePath, string fileExtension, string musicType, int percentVolume)
        {
            PercentVolume = percentVolume;
            FileExtension = fileExtension;
            MusicType = musicType;

            Init(filePath);
            GC.Collect();  // the old music file has been closed so we should definitely remove it's data from memory to free a lot of memory (> 5MB)
        }
    }
}
