// FFXIVAPP.Client
// Constants.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Common.Core.Constant;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        #region Declarations

        public static readonly string[] Supported =
        {
            "ja", "fr", "en", "de", "zh"
        };

        public static StringComparison InvariantComparer = StringComparison.InvariantCultureIgnoreCase;
        public static StringComparison CultureComparer = StringComparison.CurrentCultureIgnoreCase;

        #endregion

        #region Property Bindings

        private static Dictionary<string, ActionInfo> _actions;
        private static Dictionary<string, string> _autoTranslate;
        private static Dictionary<string, string> _chatCodes;
        private static Dictionary<string, string[]> _colors;
        private static CultureInfo _cultureInfo;
        private static string _chatCodesXml;
        private static string _characterName;
        private static string _serverName;
        private static string _gameLanguage;
        private static bool _enableNLog;
        private static bool _enableNetworkReading;
        private static bool _enableHelpLabels;
        private static string _theme;
        private static string _uiScale;

        public static Dictionary<string, ActionInfo> Actions
        {
            get { return _actions ?? (_actions = new Dictionary<string, ActionInfo>()); }
            set
            {
                _actions = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static Dictionary<string, string> AutoTranslate
        {
            get { return _autoTranslate ?? (_autoTranslate = new Dictionary<string, string>()); }
            set
            {
                _autoTranslate = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static Dictionary<string, string> ChatCodes
        {
            get { return _chatCodes ?? (_chatCodes = new Dictionary<string, string>()); }
            set
            {
                _chatCodes = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string ChatCodesXml
        {
            get { return Client.XChatCodes.ToString(); }
        }

        public static Dictionary<string, string[]> Colors
        {
            get { return _colors ?? (_colors = new Dictionary<string, string[]>()); }
            set
            {
                _colors = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static CultureInfo CultureInfo
        {
            get { return _cultureInfo ?? (_cultureInfo = new CultureInfo("en")); }
            set
            {
                _cultureInfo = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string CharacterName
        {
            get { return _characterName; }
            set
            {
                _characterName = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string GameLanguage
        {
            get { return _gameLanguage; }
            set
            {
                _gameLanguage = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableNLog
        {
            get { return _enableNLog; }
            set
            {
                _enableNLog = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableNetworkReading
        {
            get { return _enableNetworkReading; }
            set
            {
                _enableNetworkReading = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static bool EnableHelpLabels
        {
            get { return _enableHelpLabels; }
            set
            {
                _enableHelpLabels = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        public static string UIScale
        {
            get { return _uiScale; }
            set
            {
                _uiScale = value;
                ConstantsHelper.UpdatePluginConstants();
            }
        }

        #endregion

        #region Auto-Properties

        public static IntPtr ProcessHandle { get; set; }

        public static int ProcessID { get; set; }

        public static bool IsOpen { get; set; }

        public static Process[] ProcessIDs { get; set; }

        #endregion
    }
}
