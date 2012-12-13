// FFXIVAPP
// EventVM.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Windows.Input;
using FFXIVAPP.Classes;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Views;

#endregion

namespace FFXIVAPP.ViewModels
{
    public class EventVM
    {
        public EventVM()
        {
            AddEventCommand = new DelegateCommand(AddEvent);
            DeleteEventCommand = new DelegateCommand(DeleteEvent);
            EventSelectionCommand = new DelegateCommand(EventSelection);
        }

        #region GUI Functions

        /// <summary>
        /// </summary>
        private static void AddEvent()
        {
            if (Constants.XEvent.ContainsKey(EventV.View.TLine.Text))
            {
                return;
            }
            if (EventV.View.TLine.Text == "")
            {
                string title, message;
                switch (MainWindow.Lang)
                {
                    case "ja":
                        title = "情報が欠落して！";
                        message = "Line：入力してください";
                        break;
                    case "de":
                        title = "Fehlende Information!";
                        message = "Bitte Eingeben: Line";
                        break;
                    case "fr":
                        title = "Renseignements Manquants!";
                        message = "S'il Vous Plaît Entrer: Line";
                        break;
                    default:
                        title = "Missing Information!";
                        message = "Please Enter : Line";
                        break;
                }
                NotifyHelper.ShowBalloonMessage(3000, title, message);
                return;
            }
            Constants.XEvent.Add(EventV.View.TLine.Text, EventV.View.TSound.Text);
            EventV.View.Events.ItemsSource = null;
            EventV.View.Events.ItemsSource = Constants.XEvent;
            EventV.View.TLine.Text = "";
        }

        /// <summary>
        /// </summary>
        private static void DeleteEvent()
        {
            if (EventV.View.Events.SelectedIndex < 0)
            {
                return;
            }
            var item = EventV.View.Events.SelectedItem.GetType().GetProperty("Key").GetValue(EventV.View.Events.SelectedItem, null).ToString();
            Constants.XEvent.Remove(item);
            EventV.View.Events.ItemsSource = null;
            EventV.View.Events.ItemsSource = Constants.XEvent;
        }

        /// <summary>
        /// </summary>
        private static void EventSelection()
        {
            if (EventV.View.Events.SelectedItems.Count > 0)
            {
                if (EventV.View.Events.SelectedIndex < 0)
                {
                    return;
                }
                EventV.View.TLine.Text = EventV.View.Events.SelectedItem.GetType().GetProperty("Key").GetValue(EventV.View.Events.SelectedItem, null).ToString();
                EventV.View.TSound.Text = EventV.View.Events.SelectedItem.GetType().GetProperty("Value").GetValue(EventV.View.Events.SelectedItem, null).ToString();
            }
        }

        #endregion

        public ICommand AddEventCommand { get; private set; }
        public ICommand DeleteEventCommand { get; private set; }
        public ICommand EventSelectionCommand { get; private set; }
    }
}
