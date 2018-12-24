using NAudio.Wave;
using Navi.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Navi.Audio.Player
{
    /// <remarks>
    /// Some source code is taken from the tutorial of the developer of NAudio (Mark Heath) http://mark-dot-net.blogspot.de/2014/02/fire-and-forget-audio-playback-with.html
    /// </remarks>
    public class AudioData : ISampleProvider
    {
        private int currentSamplePosition;
        private int volume;
        private bool volumeIsSet;
        private WaveFormat format;

        public float[] Data { get; private set; }

        protected string FileExtension { get; set; }

        public bool Finished { get; set; }

        public WaveFormat WaveFormat { get { return format; } }

        protected int PercentVolume
        {
            get 
            { 
                return volume; 
            } 

            set 
            { 
                volumeIsSet = true;
                volume = value;
            } 
        }

        public void Init(string filePath)
        {
            if (FileExtension != null)
            {
                AudioFileReader audioReader = new AudioFileReader(filePath + FileExtension);
                StoreSettings(audioReader);
                StoreDataInMemory(audioReader);
                audioReader.Dispose();
            }
            else throw new Exceptions.NoFileFormatExtensionException();
        }

        private void StoreSettings(AudioFileReader audioReader)
        {
            if (volumeIsSet)
            {
                audioReader.Volume = ((float)PercentVolume) / 100.0f;
                format = audioReader.WaveFormat;
            }
            else throw new Exceptions.VolumeNotSetException();
        }

        private void StoreDataInMemory(AudioFileReader audioReader)
        {
            List<float> file = CreateFile(audioReader.Length);
            FillInFile(file, audioReader);
            Data = file.ToArray();
        }

        private List<float> CreateFile(long fileLengthBytes)
        {
            int twoTimesfileLengthBits = (int)(fileLengthBytes / 8) * 2;  // why 4?
            return new List<float>(twoTimesfileLengthBits);
        }

        private void FillInFile(List<float> file, AudioFileReader audioReader)
        {
            // temporary buffer
            float[] buffer = new float[format.SampleRate * format.Channels];  // sample rate: tells how many values are stored per second (example: 44100 Hz)
            int numOfsamples;  // no more than sampleRate * channels samples are read in every iteration of the following loop

            while ((numOfsamples = audioReader.Read(buffer, 0, buffer.Length)) > 0)
                file.AddRange(buffer.Take(numOfsamples));
        }

        /// <summary>
        /// Copying audio data in the destination array.
        /// </summary>
        public int Read(float[] destination, int destinationOffset, int numOfSamples)
        {
            int numRemainingSamples = this.Data.Length - currentSamplePosition;
            int numSamplesToCopy = Math.Min(numRemainingSamples, numOfSamples);  // avoids copying more samples than samples available

            Array.Copy(this.Data, currentSamplePosition, destination, destinationOffset, numSamplesToCopy);
            currentSamplePosition += numSamplesToCopy;

            if (numSamplesToCopy == 0) Finished = true;
            return numSamplesToCopy;
        }

        public void ResetSamplePosition()
        {
            currentSamplePosition = 0;
        }
    }
}
