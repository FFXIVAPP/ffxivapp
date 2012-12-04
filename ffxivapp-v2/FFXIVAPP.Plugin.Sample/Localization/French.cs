// FFXIVAPP.Plugin.Sample
// French.cs

using System.Windows;

namespace FFXIVAPP.Plugin.Sample.Localization
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
            Dictionary.Add("sample_", "*PH*");
            return Dictionary;
        }
    }
}