// FFXIVAPP.Client
// Constants.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;
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

        public static Dictionary<string, string> ServerNumber
        {
            get
            {
                var serverNumber = new Dictionary<string, string>();
                serverNumber.Add("Durandal", "2");
                serverNumber.Add("Hyperion", "3");
                serverNumber.Add("Masamune", "4");
                serverNumber.Add("Gungnir", "5");
                serverNumber.Add("Aegis", "7");
                serverNumber.Add("Sargatanas", "10");
                serverNumber.Add("Balmung", "11");
                serverNumber.Add("Ridill", "12");
                serverNumber.Add("Excalibur", "16");
                serverNumber.Add("Ragnarok", "20");
                return serverNumber;
            }
        }

        #endregion
    }
}
