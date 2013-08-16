// FFXIVAPP.Common
// BindingHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System.Windows;
using System.Windows.Data;
using FFXIVAPP.Common.Converters;

#endregion

namespace FFXIVAPP.Common.Helpers
{
    public static class BindingHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="source"> </param>
        /// <param name="path"> </param>
        /// <returns> </returns>
        public static Binding VisibilityBinding(object source, string path)
        {
            var binding = new Binding("Visibility");
            binding.Converter = new VisibilityConverter();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.TwoWay;
            return binding;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public static Binding ZoomBinding(object source, string path)
        {
            var binding = new Binding();
            binding.Source = source;
            binding.Path = new PropertyPath(path);
            binding.Mode = BindingMode.TwoWay;
            return binding;
        }
    }
}
