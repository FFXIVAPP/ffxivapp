// FFXIVAPP
// XmlWriteLog.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Collections.Generic;
using System.Xml;

namespace FFXIVAPP.Classes
{
    public class XmlWriteLog
    {
        private readonly List<string[]> _logList = new List<string[]>();
        private readonly XmlWriterSettings _xmlSettings = new XmlWriterSettings();
        public int LineCount;

        /// <summary>
        /// </summary>
        public XmlWriteLog()
        {
            _xmlSettings.Indent = true;
            _xmlSettings.IndentChars = "	";
            LineCount = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public void AddChatLine(string[] line)
        {
            _logList.Add(line);
            LineCount++;
        }

        /// <summary>
        /// </summary>
        /// <param name="path"> </param>
        public void WriteToDisk(string path)
        {
            using (var writer = XmlWriter.Create(path, _xmlSettings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Log");
                foreach (var line in _logList)
                {
                    writer.WriteStartElement("Line");
                    writer.WriteAttributeString("Value", line[0]);
                    writer.WriteAttributeString("Key", line[1]);
                    writer.WriteAttributeString("Color", line[2]);
                    writer.WriteAttributeString("Time", line[3]);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        /// <summary>
        /// </summary>
        public void ClearXml()
        {
            _logList.Clear();
            LineCount = 0;
        }
    }
}