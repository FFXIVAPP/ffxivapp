// FFXIVAPP.Client
// ZoneHelper.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Helpers
{
    [DoNotObfuscate]
    public static class ZoneHelper
    {
        private static IList<MapInfo> _mapInfoList;

        private static IEnumerable<MapInfo> MapInfoList
        {
            get { return _mapInfoList ?? (_mapInfoList = GenerateMapList()); }
        }

        public static MapInfo GetMapInfo(uint index)
        {
            var mapInfo = MapInfoList.FirstOrDefault(m => m.Index == index);
            return mapInfo ?? new MapInfo(false, index);
        }

        private static IList<MapInfo> GenerateMapList()
        {
            var mapList = new List<MapInfo>
            {
                new MapInfo(false, 128, "Limsa Lominsa Upper Decks"),
                new MapInfo(false, 129, "Limsa Lominsa Lower Decks"),
                new MapInfo(false, 130, "Ul'dah - Steps of Nald"),
                new MapInfo(false, 131, "Ul'dah - Steps of Thal"),
                new MapInfo(false, 132, "New Gridania"),
                new MapInfo(false, 133, "Old Gridania"),
                new MapInfo(false, 134, "Middle La Noscea"),
                new MapInfo(false, 135, "Lower La Noscea"),
                new MapInfo(false, 136, "Mist"),
                new MapInfo(false, 137, "Eastern La Noscea"),
                new MapInfo(false, 138, "Western La Noscea"),
                new MapInfo(false, 139, "Upper La Noscea"),
                new MapInfo(false, 140, "Western Thanalan"),
                new MapInfo(false, 141, "Central Thanalan"),
                new MapInfo(false, 145, "Eastern Thanalan"),
                new MapInfo(false, 146, "Southern Thanalan"),
                new MapInfo(false, 147, "Northern Thanalan"),
                new MapInfo(false, 148, "Central Shroud"),
                new MapInfo(false, 152, "East Shroud"),
                new MapInfo(false, 153, "South Shroud"),
                new MapInfo(false, 154, "North Shroud"),
                new MapInfo(false, 155, "Coerthas Central Highlands"),
                new MapInfo(false, 156, "Mor Dhona"),
                new MapInfo(true, 157, "Sastasha"),
                new MapInfo(true, 158, "Brayflox's Longstop"),
                new MapInfo(true, 159, "The Wanderer's Palace"),
                new MapInfo(true, 161, "Copperbell Mines"),
                new MapInfo(true, 162, "Halatali"),
                new MapInfo(true, 163, "The Sunken Temple of Qarn"),
                new MapInfo(true, 164, "The Tam-Tara Deepcroft"),
                new MapInfo(true, 166, "Haukke Manor"),
                new MapInfo(true, 167, "Amdapor Keep"),
                new MapInfo(true, 168, "Stone Vigil"),
                new MapInfo(true, 169, "The Thousand Maws of Toto-Rak"),
                new MapInfo(true, 170, "Cutter's Cry"),
                new MapInfo(true, 171, "Dzemael Darkhold"),
                new MapInfo(true, 172, "Aurum Vale"),
                new MapInfo(true, 175, "The Wolves' Den"),
                new MapInfo(false, 177, "Mizzenmast Inn"),
                new MapInfo(false, 178, "The Hourglass"),
                new MapInfo(false, 179, "The Roost"),
                new MapInfo(false, 180, "Outer La Noscea"),
                new MapInfo(false, 198, "Command Room"),
                new MapInfo(false, 199, "Unknown"),
                new MapInfo(false, 200, "Unknown"),
                new MapInfo(true, 202, "Bowl of Embers"),
                new MapInfo(false, 204, "Seat of the First Bow"),
                new MapInfo(false, 205, "Lotus Stand"),
                new MapInfo(true, 206, "The Navel"),
                new MapInfo(true, 208, "The Howling Eye"),
                new MapInfo(false, 210, "Heart of the Sworn"),
                new MapInfo(false, 211, "The Fragrant Chamber"),
                new MapInfo(false, 212, "The Waking Sands"),
                new MapInfo(true, 217, "Castrum Meridianum"),
                new MapInfo(true, 224, "Praetorium"),
                new MapInfo(false, 241, "Upper Aetheroacoustic Exploratory Site"),
                new MapInfo(false, 242, "Lower Aetheroacoustic Exploratory Site"),
                new MapInfo(false, 243, "The Ragnarok"),
                new MapInfo(false, 244, "Ragnarok Drive Cylinder"),
                new MapInfo(false, 245, "Ragnarok Central Core"),
                new MapInfo(false, 250, "Wolves' Den Pier"),
                new MapInfo(true, 331, "The Howling Eye")
            };

            return mapList;
        }

        public class MapInfo
        {
            private string _english;
            private string _french;
            private string _german;
            private string _japanese;

            /// <summary>
            /// </summary>
            /// <param name="isDunegonInstance"></param>
            /// <param name="index"></param>
            /// <param name="english"></param>
            /// <param name="french"></param>
            /// <param name="german"></param>
            /// <param name="japanese"></param>
            public MapInfo(bool isDunegonInstance, uint index = 0, string english = null, string french = null, string german = null, string japanese = null)
            {
                Index = index;
                IsDungeonInstance = isDunegonInstance;
                English = english;
                French = french;
                German = german;
                Japanese = japanese;
            }

            public uint Index { get; set; }

            public bool IsDungeonInstance { get; set; }

            public string English
            {
                get { return _english ?? String.Format("Unknown_{0}", Index); }
                set { _english = value; }
            }

            public string French
            {
                get { return _french ?? String.Format("Unknown_{0}", Index); }
                set { _french = value; }
            }

            public string German
            {
                get { return _german ?? String.Format("Unknown_{0}", Index); }
                set { _german = value; }
            }

            public string Japanese
            {
                get { return _japanese ?? String.Format("Unknown_{0}", Index); }
                set { _japanese = value; }
            }
        }
    }
}
