// FFXIVAPP.Client
// LineHistory.cs
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

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Common.Core.Memory;
using NLog;

namespace FFXIVAPP.Client.Models.Parse
{
    public class LineHistory
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        public LineHistory(Line line)
        {
            TimeStamp = DateTime.Now;
            Line = line;
            SourceStatusEntries = new List<StatusEntry>();
            TargetStatusEntries = new List<StatusEntry>();
            uint PlayerID = 0;
            try
            {
                var monsterEntries = MonsterWorkerDelegate.GetNPCEntities();
                var pcEntries = PCWorkerDelegate.GetNPCEntities();
                // process you => monster
                foreach (var actorEntity in pcEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Source, Constants.InvariantComparer))
                    {
                        continue;
                    }
                    PlayerID = actorEntity.ID;
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        SourceStatusEntries.Add(statusEntry);
                    }
                }
                foreach (var actorEntity in monsterEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Target, Constants.InvariantComparer))
                    {
                        return;
                    }
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        if (statusEntry.CasterID == PlayerID)
                        {
                            TargetStatusEntries.Add(statusEntry);
                        }
                    }
                }
                // process monster => you
                foreach (var actorEntity in pcEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Target, Constants.InvariantComparer))
                    {
                        continue;
                    }
                    PlayerID = actorEntity.ID;
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        TargetStatusEntries.Add(statusEntry);
                    }
                }
                foreach (var actorEntity in monsterEntries)
                {
                    if (!String.Equals(actorEntity.Name, line.Source, Constants.InvariantComparer))
                    {
                        return;
                    }
                    foreach (var statusEntry in actorEntity.StatusEntries)
                    {
                        if (statusEntry.CasterID == PlayerID)
                        {
                            SourceStatusEntries.Add(statusEntry);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DateTime TimeStamp { get; set; }
        public Line Line { get; set; }

        #region StatEnties [Source|Target]

        public List<StatusEntry> SourceStatusEntries { get; set; }
        public List<StatusEntry> TargetStatusEntries { get; set; }

        #endregion
    }
}
