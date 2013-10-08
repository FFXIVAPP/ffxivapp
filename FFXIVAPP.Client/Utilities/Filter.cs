// FFXIVAPP.Client
// Filter.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Enums;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;

#endregion

namespace FFXIVAPP.Client.Utilities
{
    public static partial class Filter
    {
        public static bool IsEnabled = true;

        // setup self info
        private static readonly string You = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
        private static Event _lastEventPlayer;
        private static string _lastNamePlayer = You;
        private static string _lastActionPlayer = "";

        // setup party info
        private static Event _lastEventParty;
        private static string _lastNameParty = "";
        private static string _lastActionParty = "";

        // setup monster info
        private static string _lastMobName = "";
        private static string _lastMobAction = "";

        private static bool _autoAction;
        private static bool _isMulti;
        private static bool _isParty;

        public static void Process(string cleaned, Event e)
        {
            if (!IsEnabled)
            {
                return;
            }

            _lastEventParty = _lastEventParty ?? new Event();
            _lastEventPlayer = _lastEventPlayer ?? new Event();
            _autoAction = false;
            _isParty = true;

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

            if (_isParty)
            {
                _lastEventParty = e;
            }
            else
            {
                _lastEventPlayer = e;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="clearNames"></param>
        private static void ClearLast(bool clearNames = false)
        {
            _lastActionParty = "";
            _lastMobAction = "";
            if (!clearNames)
            {
                return;
            }
            _lastNameParty = "";
            _lastMobName = "";
        }
    }
}
