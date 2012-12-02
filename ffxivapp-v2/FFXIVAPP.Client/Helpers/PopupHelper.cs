// FFXIVAPP.Client
// PopupHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Models;

namespace FFXIVAPP.Client.Helpers
{
    internal static class PopupHelper
    {
        private const int Time = 1000000000/100;

        public static Popup MessagePopup
        {
            get { return ShellView.View.Notify; }
        }

        private static DoubleAnimation BlurIn
        {
            get
            {
                var timeSpan = new TimeSpan((long) (Time*.1));
                return new DoubleAnimation(3, 0, timeSpan);
            }
        }

        private static DoubleAnimation BlurOut
        {
            get
            {
                var timeSpan = new TimeSpan((long) (Time*.1));
                return new DoubleAnimation(0, 3, timeSpan);
            }
        }

        public static MessageBoxResult Result { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="content"> </param>
        public static void Toggle(object content)
        {
            var popupContent = content as PopupContent;
            Result = MessageBoxResult.None;
            var window = ShellView.View;
            window.MetroNotify.Popup_No.Visibility = popupContent.CanSayNo ? Visibility.Visible : Visibility.Collapsed;
            switch (window.Notify.IsOpen)
            {
                case false:
                    //setup blur animation to blur MetroPopup
                    window.MetroNotify.Effect = new BlurEffect();
                    window.MetroNotify.Effect.BeginAnimation(BlurEffect.RadiusProperty, BlurIn);
                    FadeIn(window.MetroNotify.Title, .25, 0);
                    FadeIn(window.MetroNotify.Message, .5, 0);
                    //setup popup content
                    window.MetroNotify.Title.Content = popupContent.Title.ToUpperInvariant();
                    window.MetroNotify.Message.Text = popupContent.Message;
                    //set max width and location
                    window.Notify.Placement = PlacementMode.Center;
                    window.Notify.MaxWidth = (double) Math.Ceiling(((decimal) Settings.Default.Width/2));
                    //open popup
                    window.Notify.IsOpen = true;
                    //assign ClickEvent to OK button
                    RoutedEventHandler popupYes = null;
                    popupYes = delegate
                    {
                        //animation blur effect back to 0
                        window.MetroNotify.Effect.BeginAnimation(BlurEffect.RadiusProperty, BlurOut);
                        //toggle result
                        Result = MessageBoxResult.Yes;
                        //close popup
                        window.Notify.IsOpen = false;
                        window.MetroNotify.Popup_Yes.Click -= popupYes;
                    };
                    window.MetroNotify.Popup_Yes.Click += popupYes;
                    if (popupContent.CanSayNo)
                    {
                        RoutedEventHandler popupNo = null;
                        popupNo = delegate
                        {
                            //animation blur effect back to 0
                            window.MetroNotify.Effect.BeginAnimation(BlurEffect.RadiusProperty, BlurOut);
                            //toggle result
                            Result = MessageBoxResult.No;
                            //close popup
                            window.Notify.IsOpen = false;
                            window.MetroNotify.Popup_No.Click -= popupNo;
                        };
                        window.MetroNotify.Popup_No.Click += popupNo;
                    }
                    break;
            }
        }

        /// <summary>
        /// </summary>
        public static void ForceClose()
        {
            var window = ShellView.View;
            if (!window.Notify.IsOpen)
            {
                return;
            }
            Result = MessageBoxResult.None;
            //animation blur effect back to 0
            window.MetroNotify.Effect.BeginAnimation(BlurEffect.RadiusProperty, BlurOut);
            //enable MainWindow
            window.IsEnabled = true;
            //close popup
            window.Notify.IsOpen = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="framework"> </param>
        /// <param name="timeOffset"> </param>
        private static void FadeOut(UIElement framework, double timeOffset)
        {
            var animation = new DoubleAnimation(framework.Opacity, .5, new TimeSpan((long) (Time*timeOffset)));
            framework.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        /// <summary>
        /// </summary>
        /// <param name="framework"> </param>
        /// <param name="timeOffset"> </param>
        /// <param name="to"> </param>
        private static void FadeOut(UIElement framework, double timeOffset, double to)
        {
            var animation = new DoubleAnimation(framework.Opacity, to, new TimeSpan((long) (Time*timeOffset)));
            framework.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        /// <summary>
        /// </summary>
        /// <param name="framework"> </param>
        /// <param name="timeOffset"> </param>
        private static void FadeIn(UIElement framework, double timeOffset)
        {
            var animation = new DoubleAnimation(framework.Opacity, 1, new TimeSpan((long) (Time*timeOffset)));
            framework.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        /// <summary>
        /// </summary>
        /// <param name="framework"> </param>
        /// <param name="timeOffset"> </param>
        /// <param name="from"> </param>
        private static void FadeIn(UIElement framework, double timeOffset, double from)
        {
            var animation = new DoubleAnimation(from, 1, new TimeSpan((long) (Time*timeOffset)));
            framework.BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }
}