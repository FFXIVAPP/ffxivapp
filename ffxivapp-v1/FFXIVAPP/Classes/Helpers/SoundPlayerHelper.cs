// FFXIVAPP
// SoundPlayerHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Media;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class SoundPlayerHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="filename"> </param>
        public static void Play(string filename = "aruba.wav")
        {
            using (var sp = new SoundPlayer())
            {
                sp.SoundLocation = String.Format("Sounds/{0}", filename);
                sp.PlaySync();
            }
        }
    }
}