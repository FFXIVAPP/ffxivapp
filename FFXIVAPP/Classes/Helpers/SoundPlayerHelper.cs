// FFXIVAPP
// SoundPlayerHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Media;

namespace FFXIVAPP.Classes.Helpers
{
    internal class SoundPlayerHelper
    {
        private static readonly SoundPlayer SoundPlayer = new SoundPlayer();

        /// <summary>
        /// </summary>
        /// <param name="filename"> </param>
        public void Play(string filename = "aruba.wav")
        {
            SoundPlayer.SoundLocation = filename;
            SoundPlayer.PlaySync();
        }
    }
}