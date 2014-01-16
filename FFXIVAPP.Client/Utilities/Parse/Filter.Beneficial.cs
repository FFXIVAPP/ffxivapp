// FFXIVAPP.Client
// Filter.Beneficial.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Linq;
using System.Text.RegularExpressions;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Helpers.Parse;
using FFXIVAPP.Client.Models.Parse;
using FFXIVAPP.Client.Models.Parse.Events;
using FFXIVAPP.Common.Core.Memory.Enums;

namespace FFXIVAPP.Client.Utilities.Parse
{
    public static partial class Filter
    {
        private static void ProcessBeneficial(Event e, Expressions exp)
        {
            var line = new Line(e.ChatLogEntry)
            {
                EventDirection = e.Direction,
                EventSubject = e.Subject,
                EventType = e.Type
            };
            LineHelper.SetTimelineTypes(ref line);
            if (LineHelper.IsIgnored(line))
            {
                return;
            }
            var beneficial = Regex.Match("ph", @"^\.$");
            switch (e.Subject)
            {
                case EventSubject.You:
                    switch (e.Direction)
                    {
                        case EventDirection.Self:
                            line.Target = You;
                            break;
                    }
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = You;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.You);
                    }
                    break;
                case EventSubject.Pet:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNamePet;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Pet);
                    }
                    break;
                case EventSubject.Party:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNamePartyFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Party);
                    }
                    break;
                case EventSubject.PetParty:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNamePetPartyFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.PetParty);
                    }
                    break;
                case EventSubject.Alliance:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNameAllianceFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Alliance);
                    }
                    break;
                case EventSubject.PetAlliance:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNamePetAllianceFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.PetAlliance);
                    }
                    break;
                case EventSubject.Other:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNameOtherFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.Other);
                    }
                    break;
                case EventSubject.PetOther:
                    beneficial = exp.pBeneficialGain;
                    if (beneficial.Success)
                    {
                        line.Source = _lastNamePetOtherFrom;
                        UpdateBeneficialGain(beneficial, line, exp, FilterType.PetOther);
                    }
                    break;
            }
            if (beneficial.Success)
            {
                return;
            }
            ParsingLogHelper.Log(Logger, "Beneficial", e, exp);
        }

        private static void UpdateBeneficialGain(Match beneficial, Line line, Expressions exp, FilterType type)
        {
            _type = type;
            try
            {
                if (String.IsNullOrWhiteSpace(line.Source))
                {
                    line.Source = Convert.ToString(beneficial.Groups["source"].Value);
                }
                if (String.IsNullOrWhiteSpace(line.Target))
                {
                    line.Target = Convert.ToString(beneficial.Groups["target"].Value);
                }
                line.Action = Convert.ToString(beneficial.Groups["status"].Value);
                var isStoneSkin = false;
                foreach (var stoneSkin in MagicBarrierHelper.StoneSkin.Where(stoneSkin => String.Equals(stoneSkin, line.Action, Constants.InvariantComparer)))
                {
                    isStoneSkin = true;
                }
                switch (line.EventDirection)
                {
                    case EventDirection.You:
                        line.Target = You;
                        break;
                }
                if (line.IsEmpty())
                {
                    return;
                }
                if (isStoneSkin)
                {
                    var multiplier = 0.1m;
                    try
                    {
                        var cleanedName = Regex.Replace(line.Source, @"\[[\w]+\]", "")
                                               .Trim();
                        var source = PCWorkerDelegate.GetNPCEntityByName(cleanedName);
                        if (source != null)
                        {
                            multiplier = source.Job == Actor.Job.WHM ? 0.18m : multiplier;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    try
                    {
                        var cleanedName = Regex.Replace(line.Target, @"\[[\w]+\]", "")
                                               .Trim();
                        var target = PCWorkerDelegate.GetNPCEntityByName(cleanedName);
                        if (target != null)
                        {
                            line.Amount = target.HPMax * multiplier;
                            line.Action = String.Format("{0} [•]", line.Action);
                            ParseControl.Instance.Timeline.GetSetPlayer(line.Source)
                                        .SetHealingMitigated(line, "stoneskin");
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ParsingLogHelper.Error(Logger, "Cure", exp.Event, ex);
            }
        }
    }
}
