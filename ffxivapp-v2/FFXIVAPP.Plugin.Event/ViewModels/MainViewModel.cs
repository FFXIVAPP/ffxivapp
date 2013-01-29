// FFXIVAPP.Plugin.Event
// MainViewModel.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using FFXIVAPP.Common.Models;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Common.ViewModelBase;
using FFXIVAPP.Plugin.Event.Views;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Event.ViewModels
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Property Bindings

        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get { return _instance ?? (_instance = new MainViewModel()); }
        }

        #endregion

        #region Declarations

        public ICommand AddEventCommand { get; private set; }
        public ICommand DeleteEventCommand { get; private set; }
        public ICommand EventSelectionCommand { get; private set; }

        #endregion

        public MainViewModel()
        {
            AddEventCommand = new DelegateCommand(AddEvent);
            DeleteEventCommand = new DelegateCommand(DeleteEvent);
            EventSelectionCommand = new DelegateCommand(EventSelection);
        }

        #region Loading Functions

        #endregion

        #region Utility Functions

        /// <summary>
        /// </summary>
        /// <param name="listView"> </param>
        /// <param name="key"> </param>
        private static string GetValueBySelectedItem(Selector listView, string key)
        {
            var type = listView.SelectedItem.GetType();
            var property = type.GetProperty(key);
            return property.GetValue(listView.SelectedItem, null)
                           .ToString();
        }

        #endregion

        #region Command Bindings

        /// <summary>
        /// </summary>
        private static void AddEvent()
        {
            var selectedKey = "";
            try
            {
                selectedKey = GetValueBySelectedItem(MainView.View.Events, "Key");
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
            }
            if (MainView.View.TSound.Text == "" || MainView.View.TRegEx.Text == "")
            {
                return;
            }
            var valuePair = new XValuePair
            {
                Key = MainView.View.TRegEx.Text,
                Value = MainView.View.TSound.Text
            };
            if (String.IsNullOrWhiteSpace(selectedKey))
            {
                var found = PluginViewModel.Instance.Events.Any(pair => pair.Key == valuePair.Key);
                if (!found)
                {
                    PluginViewModel.Instance.Events.Add(valuePair);
                }
            }
            else
            {
                var index = PluginViewModel.Instance.Events.TakeWhile(pair => pair.Key != selectedKey)
                                           .Count();
                PluginViewModel.Instance.Events[index] = valuePair;
            }
            MainView.View.Events.UnselectAll();
            MainView.View.TRegEx.Text = "";
        }

        /// <summary>
        /// </summary>
        private static void DeleteEvent()
        {
            string selectedKey;
            try
            {
                selectedKey = GetValueBySelectedItem(MainView.View.Events, "Key");
            }
            catch (Exception ex)
            {
                Logging.Log(LogManager.GetCurrentClassLogger(), "", ex);
                return;
            }
            var index = PluginViewModel.Instance.Events.TakeWhile(pair => pair.Key != selectedKey)
                                       .Count();
            PluginViewModel.Instance.Events.RemoveAt(index);
        }

        /// <summary>
        /// </summary>
        private static void EventSelection()
        {
            if (MainView.View.Events.SelectedItems.Count != 1)
            {
                return;
            }
            if (MainView.View.Events.SelectedIndex < 0)
            {
                return;
            }
            MainView.View.TRegEx.Text = GetValueBySelectedItem(MainView.View.Events, "Key");
            MainView.View.TSound.Text = GetValueBySelectedItem(MainView.View.Events, "Value");
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion
    }
}
