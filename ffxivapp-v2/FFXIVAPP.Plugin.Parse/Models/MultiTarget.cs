// FFXIVAPP.Plugin.Parse
// MultiTarget.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Collections.Generic;

#endregion

namespace FFXIVAPP.Plugin.Parse.Models
{
    public static class MultiTarget
    {
        private static List<string> _multiTargetSkills;

        private static List<string> MultiTargetSkills
        {
            get { return _multiTargetSkills ?? (_multiTargetSkills = GetMultiTargetList()); }
            set { _multiTargetSkills = value; }
        }

        public static bool IsMulti(string action)
        {
            return MultiTargetSkills.Contains(action.ToLower());
        }

        private static List<string> GetMultiTargetList()
        {
            var result = new List<string>();
            result.Add("medica");
            return result;
        }
    }
}
