// FFXIVAPP.Common
// ResourceHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;

namespace FFXIVAPP.Common.Helpers
{
    public static class ResourceHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="key"> </param>
        /// <returns> </returns>
        public static string StringResource(string key)
        {
            return (string) Application.Current.FindResource(key);
        }

        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="field"> </param>
        /// <returns> </returns>
        public static string StringResource(object source, string field)
        {
            return (string) source.GetType()
                                  .GetField(field)
                                  .GetValue(null);
        }

        /// <summary>
        /// </summary>
        /// <param name="path"> </param>
        /// <returns> </returns>
        public static StreamResourceInfo StreamResource(string path)
        {
            return Application.GetResourceStream(new Uri(path));
        }

        /// <summary>
        /// </summary>
        /// <param name="path"> </param>
        /// <returns> </returns>
        public static XDocument XDocResource(string path)
        {
            var resource = StreamResource(path);
            return (resource == null) ? null : new XDocument(XElement.Load(resource.Stream));
        }
    }
}
