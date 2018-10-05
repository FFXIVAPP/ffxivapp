using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Logging.Serilog;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.SettingsProviders.Application;

namespace FFXIVAPP.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            LocaleHelper.Update(Settings.Default.Culture);
            BuildAvaloniaApp().Start<ShellView>(); //(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
