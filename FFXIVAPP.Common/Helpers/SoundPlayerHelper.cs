// FFXIVAPP.Common
// SoundPlayerHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Media;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using NLog;

namespace FFXIVAPP.Common.Helpers
{
    public static class SoundPlayerHelper
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

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
    }
}
