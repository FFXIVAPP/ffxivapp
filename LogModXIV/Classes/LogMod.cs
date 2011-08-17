using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

using System.Collections;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using System.Reflection;

using AppModXIV;

namespace LogModXIV.Classes
{
    public class LogMod
    {

    }

    #region " CONSTANTS "

    public class Constants_Loc
    {
        public static string[] rSettings = { "Tab", "Color", "Pos", "Size", "Translate", "Errors", "TopMost" };
        public static Dictionary<string, string> xTop = new Dictionary<string, string>();
    }

    #endregion

    #region " LANGUAGES "

    public class Languages
    {
        public string Lang { get; set; }
    }

    public class myLanguages
    {
        Boolean result = true;
        XDocument xdoc = XDocument.Load("Resources/Interface_Log.xml");

        public void LoadLanguage(string Language)
        {
            Constants.LangString.Clear();

            int mCount = 0;

            var items = from item in xdoc.Descendants("GUI")
                        select new Languages
                        {
                            Lang = (string)item.Attribute(Language),
                        };

            foreach (var item in items)
            {
                Constants.LangString.Add(mCount.ToString(), item.Lang);
                mCount++;
            }
        }

        public bool LoadLanguage(string Language, int boolCheck)
        {
            result = true;
            var items = from item in xdoc.Descendants("GUI")
                        select new Languages
                        {
                            Lang = (string)item.Attribute(Language),
                        };

            foreach (var item in items)
            {
                if (item.Lang == "")
                {
                    result = false;
                    return result;
                }
            }
            return result;
        }
    }

    #endregion
}
