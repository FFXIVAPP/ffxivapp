// FFXIVAPP.Plugin.Parse
// ParsingLogHelper.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System;
using FFXIVAPP.Common.Utilities;
using FFXIVAPP.Plugin.Parse.Models;
using FFXIVAPP.Plugin.Parse.Models.Events;
using NLog;

#endregion

namespace FFXIVAPP.Plugin.Parse.Helpers
{
    public static class ParsingLogHelper
    {
        public static void Log(Logger logger, string type, Event e, Expressions exp = null)
        {
            var cleaned = "";
            if (exp != null)
            {
                cleaned = exp.Cleaned;
            }
            var data = String.Format("Unknown {0} Line -> [Subject:{1}][Direction:{2}] {3}:{4}", type, e.Subject, e.Direction, String.Format("{0:X4}", e.Code), cleaned);
            Logging.Log(logger, data);
        }
    }
}
