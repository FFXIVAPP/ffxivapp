// FFXIVAPP.Common
// CachedSoundSampleProvider.cs
// 
// © 2013 Ryan Wilson

using System;
using NAudio.Wave;

namespace FFXIVAPP.Common.Audio
{
    public class CachedSoundSampleProvider : ISampleProvider
    {
        private readonly CachedSound _cachedSound;
        private long _position;
        private float _volume;

        public CachedSoundSampleProvider(CachedSound cachedSound, float volume = 1.0f)
        {
            _cachedSound = cachedSound;
            _volume = volume;
        }

        public float Volume
        {
            get { return _volume; }
            set { _volume = value; }
        }

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = _cachedSound.AudioData.Length - _position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(_cachedSound.AudioData, _position, buffer, offset, samplesToCopy);
            _position += samplesToCopy;
            if (_volume != 1f)
            {
                for (var n = 0; n < samplesToCopy; n++)
                {
                    buffer[offset + n] *= _volume;
                }
            }
            return (int) samplesToCopy;
        }

        public WaveFormat WaveFormat
        {
            get { return _cachedSound.WaveFormat; }
        }
    }
}
