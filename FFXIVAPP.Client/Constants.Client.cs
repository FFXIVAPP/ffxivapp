// FFXIVAPP.Client
// Constants.Client.cs
// 
// © 2013 Ryan Wilson

using System;
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
        [DoNotObfuscate]
        internal class Client
        {
            #region Property Bindings

            private static XDocument _xActions;
            private static XDocument _xAutoTranslate;
            private static XDocument _xChatCodes;
            private static XDocument _xColors;

            public static XDocument XActions
            {
                get
                {
                    var file = Path.Combine(Common.Constants.CachePath, "Configurations", "Actions.xml");
                    if (_xActions != null)
                    {
                        return _xActions;
                    }
                    try
                    {
                        var found = File.Exists(file);
                        _xActions = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/Actions.xml");
                    }
                    catch (Exception ex)
                    {
                        _xActions = ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/Actions.xml");
                    }
                    return _xActions;
                }
                set { _xActions = value; }
            }

            public static XDocument XAutoTranslate
            {
                get
                {
                    var file = Path.Combine(Common.Constants.CachePath, "Configurations", "AutoTranslate.xml");
                    if (_xAutoTranslate != null)
                    {
                        return _xAutoTranslate;
                    }
                    try
                    {
                        var found = File.Exists(file);
                        _xAutoTranslate = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/AutoTranslate.xml");
                    }
                    catch (Exception ex)
                    {
                        _xAutoTranslate = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/AutoTranslate.xml");
                    }
                    return _xAutoTranslate;
                }
                set { _xAutoTranslate = value; }
            }

            public static XDocument XChatCodes
            {
                get
                {
                    var file = Path.Combine(Common.Constants.CachePath, "Configurations", "ChatCodes.xml");
                    if (_xChatCodes != null)
                    {
                        return _xChatCodes;
                    }
                    try
                    {
                        var found = File.Exists(file);
                        _xChatCodes = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/ChatCodes.xml");
                    }
                    catch (Exception ex)
                    {
                        _xChatCodes = ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/ChatCodes.xml");
                    }
                    return _xChatCodes;
                }
                set { _xChatCodes = value; }
            }

            public static XDocument XColors
            {
                get
                {
                    var file = Path.Combine(Common.Constants.CachePath, "Configurations", "Colors.xml");
                    if (_xColors != null)
                    {
                        return _xColors;
                    }
                    try
                    {
                        var found = File.Exists(file);
                        _xColors = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/Colors.xml");
                    }
                    catch (Exception ex)
                    {
                        _xColors = ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/Colors.xml");
                    }
                    return _xColors;
                }
                set { _xColors = value; }
            }

            #endregion
        }
    }
}
