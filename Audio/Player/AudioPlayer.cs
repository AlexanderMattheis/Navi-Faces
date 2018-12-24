using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;

namespace Navi.Audio.Player
{
    public class AudioPlayer : IDisposable
    {
        private IWavePlayer outputModel;

        private MixingSampleProvider mixer;  // to play multiple sounds at same time and mix them together

        public virtual void Init(int sampleRate = 44100, int channels = 2)
        {
            outputModel = new WaveOut();  // WaveOut: output model like WasapiOut
            InitMixer(sampleRate, channels);
            InitOutputDevice();
        }

        private void InitMixer(int sampleRate, int channels)
        {
            mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channels));  // 32 bit IEEE floating point
            mixer.ReadFully = true;  // return silence even if there is no input, such you get no cawing
        }

        private void InitOutputDevice()
        {
            outputModel.Init(mixer);
            outputModel.Play();  // start playing silence
        }

        public void Play(AudioData data)
        {
            mixer.AddMixerInput(data);
        }

        public virtual void Dispose()
        {
            outputModel.Dispose();
        }
    }
}
