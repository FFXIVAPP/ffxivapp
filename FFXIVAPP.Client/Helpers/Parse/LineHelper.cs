// FFXIVAPP.Client
// LineHelper.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Models.Parse;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Helpers.Parse
{
    [DoNotObfuscate]
    public static class LineHelper
    {
        public static void SetTimelineTypes(ref Line line)
        {
            switch (line.EventSubject)
            {
                case EventSubject.You:
                case EventSubject.Pet:
                    line.SourceTimelineType = TimelineType.You;
                    break;
                case EventSubject.Party:
                case EventSubject.PetParty:
                    line.SourceTimelineType = TimelineType.Party;
                    break;
                case EventSubject.Alliance:
                case EventSubject.PetAlliance:
                    line.SourceTimelineType = TimelineType.Alliance;
                    break;
                case EventSubject.Other:
                case EventSubject.PetOther:
                    line.SourceTimelineType = TimelineType.Other;
                    break;
            }
            switch (line.EventDirection)
            {
                case EventDirection.Self:
                    switch (line.EventSubject)
                    {
                        case EventSubject.You:
                        case EventSubject.Pet:
                            line.TargetTimelineType = TimelineType.You;
                            break;
                        case EventSubject.Party:
                        case EventSubject.PetParty:
                            line.TargetTimelineType = TimelineType.Party;
                            break;
                        case EventSubject.Alliance:
                        case EventSubject.PetAlliance:
                            line.TargetTimelineType = TimelineType.Alliance;
                            break;
                        case EventSubject.Other:
                        case EventSubject.PetOther:
                            line.TargetTimelineType = TimelineType.Other;
                            break;
                    }
                    break;
                case EventDirection.You:
                case EventDirection.Pet:
                    line.TargetTimelineType = TimelineType.You;
                    break;
                case EventDirection.Party:
                case EventDirection.PetParty:
                    line.TargetTimelineType = TimelineType.Party;
                    break;
                case EventDirection.Alliance:
                case EventDirection.PetAlliance:
                    line.TargetTimelineType = TimelineType.Alliance;
                    break;
                case EventDirection.Other:
                case EventDirection.PetOther:
                    line.TargetTimelineType = TimelineType.Other;
                    break;
            }
        }

        public static bool IsIgnored(Line line)
        {
            return IgnoreType(line.SourceTimelineType) || IgnoreType(line.TargetTimelineType);
        }

        private static bool IgnoreType(TimelineType timelineType)
        {
            switch (timelineType)
            {
                case TimelineType.You:
                    return !Constants.Parse.PluginSettings.ParseYou;
                case TimelineType.Party:
                    return !Constants.Parse.PluginSettings.ParseParty;
                case TimelineType.Alliance:
                    return !Constants.Parse.PluginSettings.ParseAlliance;
                case TimelineType.Other:
                    return !Constants.Parse.PluginSettings.ParseOther;
            }
            return false;
        }
    }
}
