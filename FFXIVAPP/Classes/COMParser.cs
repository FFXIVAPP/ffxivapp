// FFXIVAPP
// COMParser.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Linq;
using System.Text;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Stats;

namespace FFXIVAPP.Classes
{
    internal static class COMParser
    {
        /// <summary>
        /// </summary>
        /// <param name="line"> </param>
        public static void Process(string line)
        {
            var cReg = Shared.ParseCOMS.Match(line);
            if (!cReg.Success)
            {
                return;
            }
            var cmd = Convert.ToString(cReg.Groups["cmd"].Value);
            var sub = Convert.ToString(cReg.Groups["sub"].Value);
            var c = "p";
            var t = "";
            switch (sub.Contains(":"))
            {
                case true:
                    c = sub.Split(':')[0];
                    sub = sub.Split(':')[1];
                    break;
            }
            var ascii = Encoding.GetEncoding("utf-16");
            switch (cmd)
            {
                case "parse":
                    switch (sub)
                    {
                        case "on":
                            Settings.Default.EnableParse = true;
                            break;
                        case "off":
                            Settings.Default.EnableParse = false;
                            break;
                        case "reset":
                            FFXIV.Instance.StatMonitor.Clear();
                            break;
                    }
                    break;
                case "show-mob":
                    var sList = new[] {"Total", "Reg", "Low", "High", "Avg", "Crit", "C Low", "C High", "C Avg"};
                    var sb = new StringBuilder();
                    StatGroup r;
                    if (FFXIV.Instance.Timeline.Monster.TryGetGroup(sub, out r))
                    {
                        foreach (var s in sList)
                        {
                            sb.AppendFormat("[{0}:{1}]", s, Math.Ceiling(r.Stats.GetStatValue(s)));
                        }
                        KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", c, sub)));
                        KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", c) + sb));
                    }
                    break;
                case "show-total":
                    switch (sub)
                    {
                        case "ability":
                            t = ResourceHelper.StringR("loc_Party");
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", c, t)));
                            foreach (var item in FFXIV.Instance.TotalA.OrderByDescending(i => int.Parse(i.Value)))
                            {
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", c) + item.Key + ": " + item.Value));
                            }
                            break;
                        case "healing":
                            t = ResourceHelper.StringR("loc_Healing");
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", c, t)));
                            foreach (var item in FFXIV.Instance.TotalH.OrderByDescending(i => int.Parse(i.Value)))
                            {
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", c) + item.Key + ": " + item.Value));
                            }
                            break;
                        case "damage":
                            t = ResourceHelper.StringR("loc_Damage");
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", c, t)));
                            foreach (var item in FFXIV.Instance.TotalD.OrderByDescending(i => int.Parse(i.Value)))
                            {
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", c) + item.Key + ": " + item.Value));
                            }
                            break;
                        case "dps":
                            t = "DPS";
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", c, t)));
                            foreach (var item in FFXIV.Instance.TotalDPS.OrderByDescending(i => Math.Ceiling(Decimal.Parse(i.Value))))
                            {
                                var amount = item.Value;
                                if (item.Value.Contains("."))
                                {
                                    amount = item.Value.Split('.')[0];
                                }
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", c) + item.Key + ": " + amount));
                            }
                            break;
                    }
                    break;
            }
        }
    }
}