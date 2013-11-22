// FFXIVAPP.Client
// Filter.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using FFXIVAPP.Client.Plugins.Parse.Enums;
using FFXIVAPP.Client.Plugins.Parse.Models;
using FFXIVAPP.Client.Properties;
using SmartAssembly.Attributes;

#endregion

namespace FFXIVAPP.Client.Plugins.Parse.Utilities
{
    [DoNotObfuscate]
    public static partial class Filter
    {
        // setup self info
        private static readonly string You = String.IsNullOrWhiteSpace(Constants.CharacterName) ? "You" : Constants.CharacterName;
        private static Models.Events.Event _lastEventPlayer;
        private static string _lastNamePlayer = You;
        private static string _lastActionPlayer = "";

        // setup party info
        private static Models.Events.Event _lastEventParty;
        private static string _lastNameParty = "";
        private static string _lastActionParty = "";


        // setup monster info
        private static string _lastMobName = "";
        private static string _lastMobAction = "";

        private static bool _autoAction;
        private static bool _lastActionIsAttack;
        private static bool _isParty;

        public static void Process(string cleaned, Models.Events.Event e)
        {
            if (!ParseControl.Instance.FirstActionFound)
            {
                ParseControl.Instance.Reset();
            }

            _lastEventParty = _lastEventParty ?? new Models.Events.Event();
            _lastEventPlayer = _lastEventPlayer ?? new Models.Events.Event();
            _autoAction = false;
            _isParty = true;

            var expressions = new Expressions(e, cleaned);

            switch (Settings.Default.StoreHistoryEvent)
            {
                case "Any":
                    ParseControl.Instance.Timeline.StoreHistoryTimer.Stop();
                    break;
                case "Damage Only":
                    if (e.Type == EventType.Damage)
                    {
                        ParseControl.Instance.Timeline.StoreHistoryTimer.Stop();
                    }
                    break;
            }

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

            switch (Settings.Default.StoreHistoryEvent)
            {
                case "Any":
                    ParseControl.Instance.Timeline.StoreHistoryTimer.Start();
                    break;
                case "Damage Only":
                    if (e.Type == EventType.Damage)
                    {
                        ParseControl.Instance.Timeline.StoreHistoryTimer.Start();
                    }
                    break;
            }
        }
    }
}
