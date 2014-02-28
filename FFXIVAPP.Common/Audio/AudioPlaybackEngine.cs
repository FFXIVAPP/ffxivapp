// FFXIVAPP.Common
// AudioPlaybackEngine.cs
// 
// © 2013 Ryan Wilson

using System;
using System.ComponentModel;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace FFXIVAPP.Common.Audio
{
    public class AudioPlaybackEngine : IDisposable
    {
        #region Property Backings

        private static AudioPlaybackEngine _instance;

        public static AudioPlaybackEngine Instance
        {
            get { return _instance ?? (_instance = new AudioPlaybackEngine()); }
            set { _instance = value; }
        }

        #endregion

        private Guid LastAudioDevice { get; set; }
        private MixingSampleProvider Mixer;
        private IWavePlayer OutputDevice;
        private int SampleRate = 44100;
        private int ChannelCount = 2;

        public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
        {
            SampleRate = sampleRate;
            ChannelCount = channelCount;
            SetupEngine();
        }

        private void SetupEngine()
        {
            //OutputDevice = new WaveOutEvent();
            //OutputDevice = new DirectSoundOut(40);
            //OutputDevice = new WasapiOut(AudioClientShareMode.Shared, true, 40);
            if (Constants.DefaultAudioDevice == Guid.Empty)
            {
                OutputDevice = new DirectSoundOut(100);
            }
            else
            {
                LastAudioDevice = Constants.DefaultAudioDevice;
                OutputDevice = new DirectSoundOut(Constants.DefaultAudioDevice, 100);
            }
            Mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(SampleRate, ChannelCount))
            {
                ReadFully = true
            };
            OutputDevice.Init(Mixer);
            OutputDevice.Play();
        }

        public void Dispose()
        {
            OutputDevice.Dispose();
        }

        public void PlaySound(CachedSound sound, int volume = 100)
        {
            if (LastAudioDevice != Constants.DefaultAudioDevice)
            {
                OutputDevice.Stop();
                SetupEngine();
            }
            Mixer.AddMixerInput(new CachedSoundSampleProvider(sound, (float) volume / 100));
        }
    }
}
