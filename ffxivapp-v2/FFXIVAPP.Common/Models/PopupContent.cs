// FFXIVAPP.Common
// PopupContent.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;

namespace FFXIVAPP.Common.Models
{
    public class PopupContent
    {
        private string _pluginName;
        private string _title;
        private string _message;

        public string PluginName
        {
            get { return (String.IsNullOrWhiteSpace(_pluginName) ? "" : _pluginName); }
            set { _pluginName = value; }
        }

        public string Title
        {
            get
            {
                const string title = "Undefined!";
                return (String.IsNullOrWhiteSpace(_title) ? title : _title);
            }
            set { _title = value; }
        }

        public string Message
        {
            get
            {
                const string message = "This message was not set by the developer.";
                return (String.IsNullOrWhiteSpace(_message) ? message : _message);
            }
            set { _message = value; }
        }

        public bool CanSayNo { get; set; }
    }
}