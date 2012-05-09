// ParseModXIV
// Commands.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;
using System.Linq;
using System.Text;
using AppModXIV.Classes;
using ParseModXIV.ViewModel;

namespace ParseModXIV.Classes
{
    internal static class Commands
    {
        public static void Process(string line)
        {
            var cReg = RegExps.Commands.Match(line);
            if (!cReg.Success)
            {
                return;
            }
            var cmd = Convert.ToString(cReg.Groups["cmd"].Value);
            var sub = Convert.ToString(cReg.Groups["sub"].Value);
            switch (cmd)
            {
                case "parse":
                    switch (sub)
                    {
                        case "on":
                            //Speech.Speak("Parsing ON.");
                            MainStatusViewModel.ToggleLogging();
                            break;
                        case "off":
                            //Speech.Speak("Parsing OFF.");
                            MainStatusViewModel.ToggleLogging();
                            break;
                        case "reset":
                            //Speech.Speak("Parsing RESET.");
                            MainStatusViewModel.ResetStats();
                            break;
                    }
                    break;
                case "show-all":
                    switch (sub)
                    {
                        case "ability":
                            var aascii = Encoding.ASCII;
                            KeyHelper.SendNotify(aascii.GetBytes("/p * Ability Stats *"));
                            foreach (var item in ParseMod.Instance.StatMonitor.TotalA.OrderByDescending(item => int.Parse(item.Value)))
                            {
                                KeyHelper.SendNotify(aascii.GetBytes("/p " + item.Key + ": " + item.Value));
                            }
                            break;
                        case "healing":
                            var hascii = Encoding.ASCII;
                            KeyHelper.SendNotify(hascii.GetBytes("/p * Healing Stats *"));
                            foreach (var item in ParseMod.Instance.StatMonitor.TotalH.OrderByDescending(item => int.Parse(item.Value)))
                            {
                                KeyHelper.SendNotify(hascii.GetBytes("/p " + item.Key + ": " + item.Value));
                            }
                            break;
                        case "damage":
                            var dascii = Encoding.ASCII;
                            KeyHelper.SendNotify(dascii.GetBytes("/p * Damage Stats *"));
                            foreach (var item in ParseMod.Instance.StatMonitor.TotalD.OrderByDescending(item => int.Parse(item.Value)))
                            {
                                KeyHelper.SendNotify(dascii.GetBytes("/p " + item.Key + ": " + item.Value));
                            }
                            break;
                    }
                    break;
            }
        }
    }
}