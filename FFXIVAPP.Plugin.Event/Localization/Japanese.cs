// FFXIVAPP.Plugin.Event
// Japanese.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Event.Localization
{
    public abstract class Japanese
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("event_PLACEHOLDER", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "加える / アップデート項目");
            Dictionary.Add("event_RegExHeader", "RegEx");
            Dictionary.Add("event_RegExLabel", "RegEx:");
            Dictionary.Add("event_SampleText", "The scout vulture readies Wing Cutter.");
            Dictionary.Add("event_SoundHeader", "Sound");
            Dictionary.Add("event_SoundLabel", "Sound:");
            Dictionary.Add("event_DelayHeader", "遅らせる (secs)");
            Dictionary.Add("event_DelayLabel", "遅らせる (secs):");
            return Dictionary;
        }
    }
}
