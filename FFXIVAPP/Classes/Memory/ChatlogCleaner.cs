// FFXIVAPP
// ChatlogCleaner.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using NLog;

namespace FFXIVAPP.Classes.Memory
{
    public static class ChatlogCleaner
    {
        private static Boolean _colorFound;
        private static readonly string[] Checks = new[] {"0020", "0021", "0023", "0027", "0028", "0046", "0047", "0048", "0049", "005C"};
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// </summary>
        /// <param name="bytes"> </param>
        /// <param name="ci"> </param>
        /// <param name="jp"> </param>
        /// <returns> </returns>
        public static string Process(byte[] bytes, CultureInfo ci, out bool jp)
        {
            jp = false;
            try
            {
                var aList = new List<byte>();
                var nList = new List<byte>();
                var check = Encoding.UTF8.GetString(bytes.Take(4).ToArray());
                for (var x = 0; x < bytes.Count(); x++)
                {
                    if (bytes[x] == 2)
                    {
                        if (bytes[x + 1] == 29 && bytes[x + 2] == 1 && bytes[x + 3] == 3)
                        {
                            x += 4;
                        }
                        else if (bytes[x + 1] == 16 && bytes[x + 2] == 1 && bytes[x + 3] == 3)
                        {
                            x += 4;
                        }
                    }
                    if (Checks.Contains(check))
                    {
                        if (bytes[x] == 2 && _colorFound == false)
                        {
                            x += 18;
                            _colorFound = true;
                        }
                        else if (bytes[x] == 2 && _colorFound)
                        {
                            x += 10;
                            _colorFound = false;
                        }
                        nList.Add(bytes[x]);
                    }
                    else
                    {
                        if (bytes[x] == 2)
                        {
                            int length = bytes[x + 2];
                            if (length == 3)
                            {
                                x = x + 3;
                                if (bytes[x] == 1)
                                {
                                    x++;
                                }
                                while (bytes[x] != 3)
                                {
                                    aList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2")));
                                    x++;
                                }
                            }
                            else
                            {
                                x = x + 5;
                                var end = length - 3;
                                for (var t = 0; t < end; t++)
                                {
                                    aList.AddRange(Encoding.UTF8.GetBytes(bytes[x].ToString("X2")));
                                    x++;
                                }
                            }
                            if (bytes[x] == 3)
                            {
                                x++;
                            }
                            string aCheckstr;
                            if (!Constants.XAtCodes.TryGetValue(Encoding.UTF8.GetString(aList.ToArray()), out aCheckstr))
                            {
                                aCheckstr = "";
                            }
                            var atbyte = (!String.IsNullOrWhiteSpace(aCheckstr)) ? Encoding.UTF8.GetBytes(aCheckstr) : aList.ToArray();
                            nList.AddRange(atbyte);
                            aList.Clear();
                        }
                        else
                        {
                            if (bytes[x] > 127)
                            {
                                jp = true;
                            }
                            nList.Add(bytes[x]);
                        }
                    }
                }
                var cleaned = HttpUtility.HtmlDecode(Encoding.UTF8.GetString(nList.ToArray())).Replace("  ", " ");
                aList.Clear();
                nList.Clear();
                var jpc = (ci.TwoLetterISOLanguageName == "ja");
                return jpc ? HttpUtility.HtmlDecode(Encoding.UTF8.GetString(bytes.ToArray())).Replace("  ", " ") : cleaned;
            }
            catch (Exception ex)
            {
                Logger.Warn("{0} :\n{1}", ex.Message, ex.StackTrace);
                return HttpUtility.HtmlDecode(Encoding.UTF8.GetString(bytes.ToArray())).Replace("  ", " ");
            }
        }
    }
}