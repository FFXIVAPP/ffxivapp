// FFXIVAPP.Client
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.IO;
using System.Xml.Linq;
using FFXIVAPP.Common.Helpers;

#endregion

namespace FFXIVAPP.Client
{
    public static class Constants
    {
        #region Property Bindings

        private static XDocument _xAutoTranslate;
        private static XDocument _xChatCodes;
        private static XDocument _xColors;

        public static XDocument XAutoTranslate
        {
            get
            {
                if (_xAutoTranslate == null)
                {
                    const string file = "./Configurations/AutoTranslate.xml";
                    var found = File.Exists(file);
                    _xAutoTranslate = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/AutoTranslate.xml");
                }
                return _xAutoTranslate;
            }
            set { _xAutoTranslate = value; }
        }

        public static XDocument XChatCodes
        {
            get
            {
                if (_xChatCodes == null)
                {
                    const string file = "./Configurations/ChatCodes.xml";
                    var found = File.Exists(file);
                    _xChatCodes = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Resources/ChatCodes.xml");
                }
                return _xChatCodes;
            }
            set { _xChatCodes = value; }
        }

        public static XDocument XColors
        {
            get
            {
                if (_xColors == null)
                {
                    const string file = "./Configurations/Colors.xml";
                    var found = File.Exists(file);
                    _xColors = found ? XDocument.Load(file) : ResourceHelper.XDocResource(Common.Constants.AppPack + "Defaults/Colors.xml");
                }
                return _xColors;
            }
            set { _xColors = value; }
        }

        #endregion
    }
}
