// FFXIVAPP.Plugin.Parse
// Filter.cs
// 
// © 2013 Ryan Wilson

#region Usings

using FFXIVAPP.Plugin.Parse.Enums;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;

#endregion

namespace FFXIVAPP.Plugin.Parse.Utilities
{
    public static partial class Filter
    {
        public static bool IsEnabled = true;

        private static Event _lastEvent;
        private static string _lastPlayer = "";
        private static string _lastPlayerAction = "";
        private static string _lastMob = "";
        private static string _lastMobAction = "";
        private static bool _autoAction;
        private static bool _isMulti;

        public static void Process(string cleaned, Event e)
        {
            if (!IsEnabled)
            {
                return;
            }

            _lastEvent = _lastEvent ?? e;
            _autoAction = false;

            var expressions = new Expressions(e, cleaned);

            switch (e.Type)
            {
                case EventType.Damage:
                    ProcessDamage(e, expressions);
                    break;
                case EventType.Failed:
                    ProcessFailed(e, expressions);
                    break;
                case EventType.Actions:
                    ProcessActions(e, expressions);
                    break;
                case EventType.Items:
                    ProcessItems(e, expressions);
                    break;
                case EventType.Cure:
                    ProcessCure(e, expressions);
                    break;
                case EventType.Beneficial:
                    ProcessBeneficial(e, expressions);
                    break;
                case EventType.Detrimental:
                    ProcessDetrimental(e, expressions);
                    break;
            }

            _lastEvent = e;
        }

        /// <summary>
        /// </summary>
        /// <param name="clearNames"></param>
        private static void ClearLast(bool clearNames = false)
        {
            _lastPlayerAction = "";
            _lastMobAction = "";
            if (!clearNames)
            {
                return;
            }
            _lastPlayer = "";
            _lastMob = "";
        }
    }
}
