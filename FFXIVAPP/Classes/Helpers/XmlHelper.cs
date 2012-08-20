// FFXIVAPP
// XmlHelper.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System.Linq;
using System.Xml.Linq;

namespace FFXIVAPP.Classes.Helpers
{
    internal static class XmlHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="xDoc"> </param>
        /// <param name="mNode"> </param>
        /// <param name="key"> </param>
        /// <param name="value"> </param>
        public static void UpdateXMLNode(XDocument xDoc, string mNode, string key, string value)
        {
            xDoc.Descendants(mNode).Single(x =>
            {
                var xAttribute = x.Attribute("Key");
                return xAttribute != null && xAttribute.Value == key;
            }).SetAttributeValue("Value", value);
        }

        /// <summary>
        /// </summary>
        /// <param name="xDoc"> </param>
        /// <param name="mNode"> </param>
        /// <param name="mRoot"> </param>
        /// <param name="key"> </param>
        /// <param name="value"> </param>
        /// <param name="regex"> </param>
        public static void SaveXMLNode(XDocument xDoc, string mNode, string mRoot, string key, string value, string desc = "", string regex = "")
        {
            var xElement = xDoc.Element(mRoot);
            if (xElement != null)
            {
                switch (mNode)
                {
                    case "Tab":
                        if (regex == "")
                        {
                            regex = "*";
                        }
                        xElement.Add(new XElement(mNode, new XAttribute("Key", key), new XAttribute("Value", value), new XAttribute("RegEx", regex)));
                        break;
                    case "Color":
                        xElement.Add(new XElement(mNode, new XAttribute("Key", key), new XAttribute("Value", value), new XAttribute("Desc", desc)));
                        break;
                    default:
                        xElement.Add(new XElement(mNode, new XAttribute("Key", key), new XAttribute("Value", value)));
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="xDoc"> </param>
        /// <param name="mNode"> </param>
        public static void DeleteXMLNode(XDocument xDoc, string mNode)
        {
            var q = from node in xDoc.Descendants(mNode) select node;
            q.ToList().ForEach(x => x.Remove());
        }
    }
}