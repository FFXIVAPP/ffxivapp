// FFXIVAPP
// XmlCleaner.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

using System;
using System.Linq;
using System.Text;

namespace FFXIVAPP.Classes
{
    public static class XmlCleaner
    {
        /// <summary>
        /// </summary>
        /// <param name="xml"> </param>
        /// <returns> </returns>
        public static string SanitizeXmlString(string xml)
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }
            var buffer = new StringBuilder(xml.Length);
            foreach (var c in xml.Where(c => IsLegalXmlChar(c)))
            {
                buffer.Append(c);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="character"> </param>
        /// <returns> </returns>
        private static bool IsLegalXmlChar(int character)
        {
            return (character == 0x9 || character == 0xA || character == 0xD || (character >= 0x20 && character <= 0xD7FF) || (character >= 0xE000 && character <= 0xFFFD) || (character >= 0x10000 && character <= 0x10FFFF));
        }
    }
}