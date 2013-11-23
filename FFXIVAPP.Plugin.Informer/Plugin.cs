// FFXIVAPP.Plugin.Informer
// Plugin.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using FFXIVAPP.Common.Core;
using FFXIVAPP.Common.Core.Constant;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Events;
using FFXIVAPP.Common.Helpers;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.IPluginInterface;
using FFXIVAPP.Plugin.Informer.Helpers;
using FFXIVAPP.Plugin.Informer.Properties;
using FFXIVAPP.Plugin.Informer.ViewModels;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Informer
{
    [Export(typeof (IPlugin))]
    public class Plugin : IPlugin, INotifyPropertyChanged
    {
        #region Property Bindings

        public static IPluginHost PHost { get; private set; }
        public static string PName { get; private set; }

        public IApplicationContext ApplicationContext { get; set; }

        #endregion

        #region Declarations

        #endregion

        private IPluginHost _host;
        private Dictionary<string, string> _locale;
        private string _name;
        private MessageBoxResult _popupResult;

        public MessageBoxResult PopupResult
        {
            get { return _popupResult; }
            set
            {
                _popupResult = value;
                PluginViewModel.Instance.OnPopupResultChanged(new PopupResultEvent(value));
            }
        }

        public IPluginHost Host
        {
            get { return _host; }
            set { PHost = _host = value; }
        }

        public Dictionary<string, string> Locale
        {
            get { return _locale ?? (_locale = new Dictionary<string, string>()); }
            set
            {
                _locale = value;
                var locale = LocaleHelper.Update(Constants.CultureInfo);
                foreach (var resource in locale)
                {
                    try
                    {
                        _locale.Add(resource.Key, resource.Value);
                    }
                    catch (Exception ex)
                    {
                        Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                    }
                }
                PluginViewModel.Instance.Locale = _locale;
                RaisePropertyChanged();
            }
        }

        public string FriendlyName { get; set; }

        public string Name
        {
            get { return _name; }
            private set { PName = _name = value; }
        }

        public string Icon { get; private set; }

        public string Description { get; private set; }

        public string Copyright { get; private set; }

        public string Version { get; private set; }

        public string Notice { get; private set; }

        public Exception Trace { get; private set; }

        public void Initialize(IApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
            ApplicationContext.ConstantWorker.OnNewValues += ConstantWorkerOnNewValues;
            ApplicationContext.MonsterWorker.OnNewEntities += MonsterWorkerOnNewEntities;
            ApplicationContext.NPCWorker.OnNewEntities += NPCWorkerOnNewEntities;
            ApplicationContext.PCWorker.OnNewEntities += PCWorkerOnNewEntities;
            FriendlyName = "Informer";
            Name = AssemblyHelper.Name;
            Icon = "Logo.png";
            Description = AssemblyHelper.Description;
            Copyright = AssemblyHelper.Copyright;
            Version = AssemblyHelper.Version.ToString();
            Notice = "";
        }

        public void Dispose(bool isUpdating = false)
        {
            /*
             * If the isUpdating is true it means the application will be force closing/killed.
             * You wil have to choose what you want to do in this case.
             * By default the settings class clears the settings object and recreates it; but if killed untimely it will not save.
             * 
             * Suggested use is to not save settings if updating. Other disposing events could happen based on your needs.
             */
            Settings.Default.Save();
        }

        public TabItem CreateTab()
        {
            var content = new ShellView();
            content.Loaded += ShellViewModel.Loaded;
            var tabItem = new TabItem
            {
                Header = Name,
                Content = content
            };
            //do your gui stuff here
            //content gives you access to the base xaml
            return tabItem;
        }

        public UserControl CreateControl()
        {
            var content = new ShellView();
            content.Loaded += ShellViewModel.Loaded;
            //do your gui stuff here
            //content gives you access to the base xaml
            return content;
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        private void ConstantWorkerOnNewValues(ConstantEntry constantEntry)
        {
            // delegate event from constants, not required to subsribe, but recommended as it gives you app settings
            Constants.AutoTranslate = constantEntry.AutoTranslate;
            Constants.ChatCodes = constantEntry.ChatCodes;
            Constants.Colors = constantEntry.Colors;
            Constants.CultureInfo = constantEntry.CultureInfo;
            Constants.CharacterName = constantEntry.CharacterName;
            Constants.ServerName = constantEntry.ServerName;
            Constants.GameLanguage = constantEntry.GameLanguage;
            Constants.EnableHelpLabels = constantEntry.EnableHelpLabels;
            PluginViewModel.Instance.EnableHelpLabels = Constants.EnableHelpLabels;
        }

        private void MonsterWorkerOnNewEntities(List<ActorEntity> actorEntities)
        {
            // delegate event from monster entities from ram, not required to subsribe
            XIVInfoViewModel.Instance.CurrentMonsters = actorEntities;
        }

        private void NPCWorkerOnNewEntities(List<ActorEntity> actorEntities)
        {
            // delegate event from npc entities from ram, not required to subsribe; this list includes anything that is not a player or monster
            XIVInfoViewModel.Instance.CurrentNPCs = actorEntities;
        }

        private void PCWorkerOnNewEntities(List<ActorEntity> actorEntities)
        {
            // delegate event from player entities from ram, not required to subsribe
            XIVInfoViewModel.Instance.CurrentPCs = actorEntities;
            if (actorEntities.Any())
            {
                XIVInfoViewModel.Instance.CurrentUser = actorEntities.First();
            }
        }
    }
}
