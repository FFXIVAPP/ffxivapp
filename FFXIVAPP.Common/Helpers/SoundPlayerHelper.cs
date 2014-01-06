// FFXIVAPP.Common
// SoundPlayerHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Utilities;
using NAudio.Wave;
using NLog;

namespace FFXIVAPP.Common.Helpers
{
    public static class SoundPlayerHelper
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region SoundFiles Storage - Getters & Setters

        public static List<string> GetSoundFiles()
        {
            lock (SoundFiles)
            {
                if (SoundFiles.Any())
                {
                    return SoundFiles.Select(soundFile => soundFile.Key)
                                     .ToList();
                }
                return new List<string>();
            }
        }

        public static Tuple<IWavePlayer, WaveChannel32> TryGetSetSoundFile(string soundFile, int volume = 100)
        {
            lock (SoundFiles)
            {
                IWavePlayer player;
                WaveChannel32 stream;
                Tuple<IWavePlayer, WaveChannel32> value;
                if (SoundFiles.TryGetValue(soundFile, out value))
                {
                    player = value.Item1;
                    stream = value.Item2;
                    stream.Position = 0;
                }
                else
                {
                    player = new WaveOut();
                    stream = LoadStream(Path.Combine(Constants.SoundsPath, soundFile));
                    player.Init(stream);
                    SoundFiles.Add(soundFile, Tuple.Create(player, stream));
                }
                stream.Volume = (float)volume / 100;
                return new Tuple<IWavePlayer, WaveChannel32>(player, stream);
            }
        }

        private static readonly Dictionary<string, Tuple<IWavePlayer, WaveChannel32>> SoundFiles = new Dictionary<string, Tuple<IWavePlayer, WaveChannel32>>();

        #endregion

        [DllImport("winmm.dll")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);

        /// <summary>
        /// </summary>
        /// <param name="path"> </param>
        /// <param name="filename"> </param>
        public static void Play(string path = "Sounds/", string filename = "aruba.wav")
        {
            using (var soundPlayer = new SoundPlayer(path + filename))
            {
                try
                {
                    soundPlayer.PlaySync();
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="soundFile"></param>
        /// <param name="volume"></param>
        public static bool PlayCached(string soundFile, int volume = 100)
        {
            var success = true;
            try
            {
                var soundTuple = TryGetSetSoundFile(soundFile, volume);
                var player = soundTuple.Item1;
                player.Play();
                player.PlaybackStopped += delegate { player.Dispose(); };
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, "", ex);
                success = false;
            }
            return success;
        }

        /// <summary>
        /// </summary>
        public static void CacheSoundFiles()
        {
            try
            {
                if (!Directory.Exists(Constants.SoundsPath))
                {
                    Directory.CreateDirectory(Constants.SoundsPath);
                }
                var files = Directory.GetFiles(Constants.SoundsPath)
                                     .Where(file => Regex.IsMatch(file, @"^.+\.(wav|mp3)$"))
                                     .Select(file => new FileInfo(file));
                foreach (var soundFile in files.Where(soundFile => !GetSoundFiles().Contains(soundFile.Name)))
                {
                    TryGetSetSoundFile(soundFile.Name);
                }
            }
            catch (Exception ex)
            {
                Logging.Log(Logger, "", ex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static WaveChannel32 LoadStream(string filePath)
        {
            if (File.Exists(filePath))
            {
                if (filePath.EndsWith("mp3"))
                {
                    return new WaveChannel32(new Mp3FileReader(filePath));
                }
                if (filePath.EndsWith("wav"))
                {
                    return new WaveChannel32(new WaveFileReader(filePath));
                }
            }
            throw new Exception("Invalid Sound File: " + filePath);
        }
    }
}
