// FFXIVAPP.Common
// XmlHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FFXIVAPP.Common.Models;

#endregion

namespace FFXIVAPP.Common.Helpers
{
    public static class XmlHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="xDoc"> </param>
        /// <param name="xRoot"> </param>
        /// <param name="xNode"> </param>
        /// <param name="xKey"> </param>
        /// <param name="xValuePairs"> </param>
        public static void SaveXmlNode(XDocument xDoc, string xRoot, string xNode, string xKey, IEnumerable<XValuePair> xValuePairs)
        {
            var element = xDoc.Element(xRoot);
            if (element == null)
            {
                return;
            }
            var newElement = new XElement(xNode, new XAttribute("Key", xKey));
            foreach (var s in xValuePairs)
            {
                newElement.Add(new XElement(s.Key, SanitizeXmlString(s.Value)));
            }
            element.Add(newElement);
        }

        /// <summary>
        /// </summary>
        /// <param name="xDoc"> </param>
        /// <param name="xNode"> </param>
        public static void DeleteXmlNode(XDocument xDoc, string xNode)
        {
            var query = from node in xDoc.Descendants(xNode) select node;
            query.ToList()
                 .ForEach(node => node.Remove());
        }

        public static string GetValue(XDocument xDoc, string xElement, string xKey, string xValue)
        {
            var items = xDoc.Descendants()
                            .Elements(xElement)
                            .Where(element => (string) element.Attribute("Key") == xKey);
            return (string) items.First()
                                 .Element(xValue);
        }

        /// <summary>
        /// </summary>
        /// <param name="xValue"> </param>
        /// <returns> </returns>
        public static string SanitizeXmlString(string xValue)
        {
            if (xValue == null)
            {
                throw new ArgumentNullException("xValue");
            }
            var buffer = new StringBuilder(xValue.Length);
            foreach (var xChar in xValue.Where(xChar => IsLegalXmlChar(xChar)))
            {
                buffer.Append(xChar);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="xChar"> </param>
        /// <returns> </returns>
        private static bool IsLegalXmlChar(int xChar)
        {
            return (xChar == 0x9 || xChar == 0xA || xChar == 0xD || (xChar >= 0x20 && xChar <= 0xD7FF) || (xChar >= 0xE000 && xChar <= 0xFFFD) || (xChar >= 0x10000 && xChar <= 0x10FFFF));
        }
    }
}
