// FFXIVAPP.Client
// ApplicationContextHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Common.Core;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class ApplicationContextHelper
    {
        private static IApplicationContext ApplicationContext { get; set; }

        public static IApplicationContext GetContext()
        {
            return ApplicationContext ?? (ApplicationContext = new ApplicationContext());
        }
    }
}
