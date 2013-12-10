// FFXIVAPP.Common
// PopupContent.cs
// 
// © 2013 Ryan Wilson

using System;

namespace FFXIVAPP.Common.Models
{
    public class PopupContent
    {
        private string _message;
        private string _pluginName;
        private string _title;

        public string PluginName
        {
            get { return (String.IsNullOrWhiteSpace(_pluginName) ? "UnknownPlugin" : _pluginName); }
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

        public bool CanCancel { get; set; }
    }
}
