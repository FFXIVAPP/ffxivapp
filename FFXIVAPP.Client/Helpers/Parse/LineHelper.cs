// FFXIVAPP.Client
// LineHelper.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using FFXIVAPP.Client.Enums.Parse;
using FFXIVAPP.Client.Models.Parse;

namespace FFXIVAPP.Client.Helpers.Parse
{
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
