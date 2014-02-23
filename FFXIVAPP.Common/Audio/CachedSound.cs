// FFXIVAPP.Common
// CachedSound.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using System.Linq;
using NAudio.Wave;

namespace FFXIVAPP.Common.Audio
{
    public class CachedSound
    {
        public CachedSound(string audioFileName)
        {
            using (var audioFileReader = new AudioFileReader(audioFileName))
            {
                WaveFormat = audioFileReader.WaveFormat;
                if (WaveFormat.SampleRate != 44100 || WaveFormat.Channels != 2)
                {
                    using (var resampled = new ResamplerDmoStream(audioFileReader, WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)))
                    {
                        var resampledSampleProvider = resampled.ToSampleProvider();
                        WaveFormat = resampledSampleProvider.WaveFormat;
                        var wholeFile = new List<float>((int) (resampled.Length));
                        var readBuffer = new float[resampled.WaveFormat.SampleRate * resampled.WaveFormat.Channels];
                        int samplesRead;
                        while ((samplesRead = resampledSampleProvider.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            wholeFile.AddRange(readBuffer.Take(samplesRead));
                        }
                        AudioData = wholeFile.ToArray();
                    }
                }
                else
                {
                    var wholeFile = new List<float>((int) (audioFileReader.Length / 4));
                    var readBuffer = new float[audioFileReader.WaveFormat.SampleRate * audioFileReader.WaveFormat.Channels];
                    int samplesRead;
                    while ((samplesRead = audioFileReader.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        wholeFile.AddRange(readBuffer.Take(samplesRead));
                    }
                    AudioData = wholeFile.ToArray();
                }
            }
        }

        public float[] AudioData { get; private set; }
        public WaveFormat WaveFormat { get; private set; }
    }
}
