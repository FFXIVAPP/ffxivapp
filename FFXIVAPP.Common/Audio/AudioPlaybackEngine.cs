// FFXIVAPP.Common
// AudioPlaybackEngine.cs
// 
// © 2013 Ryan Wilson

using System;
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

        private readonly MixingSampleProvider _mixer;
        private readonly IWavePlayer _outputDevice;


        public AudioPlaybackEngine(int sampleRate = 44100, int channelCount = 2)
        {
            //outputDevice = new WaveOutEvent();
            //outputDevice = new DirectSoundOut(40);
            //outputDevice = new WasapiOut(AudioClientShareMode.Shared, true, 40);
            _outputDevice = new DirectSoundOut(100);
            _mixer = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, channelCount));
            _mixer.ReadFully = true;
            _outputDevice.Init(_mixer);
            _outputDevice.Play();
        }

        public void Dispose()
        {
            _outputDevice.Dispose();
        }

        public void PlaySound(CachedSound sound, int volume = 100)
        {
            _mixer.AddMixerInput(new CachedSoundSampleProvider(sound, (float) volume / 100));
        }
    }
}
