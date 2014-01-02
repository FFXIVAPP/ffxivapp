// FFXIVAPP.Client
// MagicBarrierHelper.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Plugins.Parse.Helpers
{
    [DoNotObfuscate]
    public static class MagicBarrierHelper
    {
        private static List<string> _stoneSkin;
        private static List<string> _succor;
        private static List<string> _adloquium;

        public static List<string> StoneSkin
        {
            get
            {
                if (_stoneSkin != null)
                {
                    return _stoneSkin;
                }
                _stoneSkin = new List<string>
                {
                    "stoneskin",
                    "steinhaut",
                    "cuirasse",
                    "ストンスキン"
                };
                return _stoneSkin;
            }
        }

        public static List<string> Succor
        {
            get
            {
                if (_succor != null)
                {
                    return _succor;
                }
                _succor = new List<string>
                {
                    "succor",
                    "kurieren",
                    "traité du soulagement",
                    "士気高揚の策"
                };
                return _succor;
            }
        }

        public static List<string> Adloquium
        {
            get
            {
                if (_adloquium != null)
                {
                    return _adloquium;
                }
                _adloquium = new List<string>
                {
                    "adloquium",
                    "traité du réconfort",
                    "鼓舞激励の策"
                };
                return _adloquium;
            }
        }
    }
}
