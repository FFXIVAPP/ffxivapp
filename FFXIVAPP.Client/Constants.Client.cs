// FFXIVAPP.Client
// Constants.Client.cs
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
using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Client
{
    public static partial class Constants
    {
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
