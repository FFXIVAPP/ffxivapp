// FFXIVAPP.Plugin.Event
// French.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;

#endregion

namespace FFXIVAPP.Plugin.Event.Localization
{
    public abstract class French
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("event_PLACEHOLDER", "*PH*");
            Dictionary.Add("event_AddUpdateEventButtonText", "Ajouter ou mettre à jour un évenement");
            Dictionary.Add("event_RegExHeader", "RegEx");
            Dictionary.Add("event_RegExLabel", "RegEx:");
            Dictionary.Add("event_SampleText", "The scout vulture readies Wing Cutter.");
            Dictionary.Add("event_SoundHeader", "Son");
            Dictionary.Add("event_SoundLabel", "Son:");
            Dictionary.Add("event_DelayHeader", "Délai (secs)");
            Dictionary.Add("event_DelayLabel", "Délai (secs):");
            return Dictionary;
        }
    }
}
