using Navi.Defaults;

namespace Navi.Audio.Player
{
    public sealed class SoundData : AudioData
    {
        public SoundData(string filePath, int percentVolume = 100)
        {
            CreateData(filePath, FileExtensions.Sounds, percentVolume);
        }

        public SoundData(string filePath, string fileExtension, int percentVolume = 100)
        {
            CreateData(filePath, fileExtension, percentVolume);
        }

        private void CreateData(string filePath, string fileExtension, int percentVolume)
        {
            PercentVolume = percentVolume;
            FileExtension = fileExtension;

            Init(filePath);
        }
    }
}
