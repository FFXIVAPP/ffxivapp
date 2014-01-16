// FFXIVAPP.Client
// LineHistory.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Delegates;
using FFXIVAPP.Common.Core.Memory;
using NLog;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Models.Parse
{
    [DoNotObfuscate]
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
