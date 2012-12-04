// FFXIVAPP.Plugin.Sample
// German.cs

using System.Windows;

namespace FFXIVAPP.Plugin.Sample.Localization
{
    public abstract class German
    {
        private static readonly ResourceDictionary Dictionary = new ResourceDictionary();

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static ResourceDictionary Context()
        {
            Dictionary.Clear();
            Dictionary.Add("sample_", "*PH*");
            return Dictionary;
        }
    }
}