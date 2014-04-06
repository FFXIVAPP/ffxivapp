// FFXIVAPP.Client
// Constants.Parse.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
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
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Client.SettingsProviders.Parse;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        public static class Parse
        {
            public static Settings PluginSettings
            {
                get { return SettingsProviders.Parse.Settings.Default; }
            }

            #region Declarations

            public static readonly List<string> Abilities = new List<string>
            {
                "142B",
                "14AB",
                "152B",
                "15AB",
                "162B",
                "16AB",
                "172B",
                "17AB",
                "182B",
                "18AB",
                "192B",
                "19AB",
                "1A2B",
                "1AAB",
                "1B2B",
                "1BAB"
            };

            public static readonly List<string> NeedGreed = new List<string>
            {
                "rolls Need",
                "rolls Greed",
                "dés Besoin",
                "dés Cupidité"
            };

            #endregion

            #region Property Bindings

            private static XDocument _xSettings;
            private static XDocument _xRegEx;
            private static List<string> _settings;

            public static XDocument XSettings
            {
                get
                {
                    var file = Path.Combine(AppViewModel.Instance.PluginsSettingsPath, "FFXIVAPP.Plugin.Parse.xml");
                    var legacyFile = "./Settings/Settings.Parse.xml";
                    if (_xSettings != null)
                    {
                        return _xSettings;
                    }
                    try
                    {
                        var found = File.Exists(file);
                        if (found)
                        {
                            _xSettings = XDocument.Load(file);
                        }
                        else
                        {
                            found = File.Exists(legacyFile);
                            _xSettings = found ? XDocument.Load(legacyFile) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.xml");
                        }
                    }
                    catch (Exception ex)
                    {
                        _xSettings = ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/Settings.xml");
                    }
                    return _xSettings;
                }
                set { _xSettings = value; }
            }

            public static XDocument XRegEx
            {
                get
                {
                    var file = Path.Combine(AppViewModel.Instance.ConfigurationsPath, "RegularExpressions.xml");
                    if (_xRegEx != null)
                    {
                        return _xRegEx;
                    }
                    try
                    {
                        var found = File.Exists(file);
                        _xRegEx = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/RegularExpressions.xml");
                    }
                    catch (Exception ex)
                    {
                        _xRegEx = ResourceHelper.XDocResource(Common.Constants.AppPack + "/Defaults/RegularExpressions.xml");
                    }
                    return _xRegEx;
                }
                set { _xRegEx = value; }
            }

            public static List<string> Settings
            {
                get { return _settings ?? (_settings = new List<string>()); }
                set { _settings = value; }
            }

            #endregion
        }
    }
}
