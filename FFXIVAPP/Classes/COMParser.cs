// FFXIVAPP
// COMParser.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFXIVAPP.Classes.Helpers;
using FFXIVAPP.Classes.RegExs;
using FFXIVAPP.Stats;
using FFXIVAPP.ViewModels;
using NLog;

namespace FFXIVAPP.Classes
{
    internal static class COMParser
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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
            var cmd = cReg.Groups["cmd"].Success ? cReg.Groups["cmd"].Value : "";
            var cm = cReg.Groups["cm"].Success ? cReg.Groups["cm"].Value : "p";
            var sub = cReg.Groups["sub"].Success ? cReg.Groups["sub"].Value : "";
            var limit = cReg.Groups["limit"].Success ? Convert.ToInt32(cReg.Groups["limit"].Value) : 1000;
            limit = (limit == 0) ? 1000 : limit;
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
                            MainVM.ClearStats();
                            break;
                    }
                    break;
                case "show-mob":
                    var results = new Dictionary<string, int>();
                    KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", cm, sub)));
                    foreach (var player in FFXIV.Instance.Timeline.Party)
                    {
                        StatGroup m;
                        if (player.TryGetGroup("Monsters", out m))
                        {
                            foreach (var stats in m.Where(s => s.Name == sub).Select(r => r.Stats))
                            {
                                results.Add(player.Name, (int) Math.Ceiling(stats.GetStatValue("Total")));
                            }
                        }
                    }
                    foreach (var item in results.OrderByDescending(i => i.Value))
                    {
                        KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", cm) + item.Key + ": " + item.Value));
                    }
                    break;
                case "show-total":
                    string t;
                    switch (sub)
                    {
                        case "ability":
                            t = ResourceHelper.StringR("loc_Party");
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", cm, t)));
                            foreach (var item in FFXIV.Instance.TotalA.OrderByDescending(i => int.Parse(i.Value)).Take(limit))
                            {
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", cm) + item.Key + ": " + item.Value));
                            }
                            break;
                        case "healing":
                            t = ResourceHelper.StringR("loc_Healing");
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", cm, t)));
                            foreach (var item in FFXIV.Instance.TotalH.OrderByDescending(i => int.Parse(i.Value)).Take(limit))
                            {
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", cm) + item.Key + ": " + item.Value));
                            }
                            break;
                        case "damage":
                            t = ResourceHelper.StringR("loc_Damage");
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", cm, t)));
                            foreach (var item in FFXIV.Instance.TotalD.OrderByDescending(i => int.Parse(i.Value)).Take(limit))
                            {
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", cm) + item.Key + ": " + item.Value));
                            }
                            break;
                        case "dps":
                            t = "DPS";
                            KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} * {1} *", cm, t)));
                            foreach (var item in FFXIV.Instance.TotalDPS.OrderByDescending(i => Math.Ceiling(Decimal.Parse(i.Value))).Take(limit))
                            {
                                var amount = (item.Value.Contains(".")) ? item.Value.Split('.')[0] : item.Value;
                                KeyHelper.SendNotify(ascii.GetBytes(String.Format("/{0} ", cm) + item.Key + ": " + amount));
                            }
                            break;
                    }
                    break;
            }
        }
    }
}