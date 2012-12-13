// FFXIVAPP
// SpeechHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Speech.Synthesis;

#endregion

namespace FFXIVAPP.Classes.Helpers
{
    internal static class SpeechHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="message"> </param>
        public static void Speak(string message)
        {
            using (var synthesizer = new SpeechSynthesizer())
            {
                synthesizer.SetOutputToDefaultAudioDevice();
                var builder = new PromptBuilder();
                builder.AppendText(message);
                synthesizer.Speak(builder);
            }
        }
    }
}
