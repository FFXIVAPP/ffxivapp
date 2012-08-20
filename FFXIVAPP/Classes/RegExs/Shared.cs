// FFXIVAPP
// Shared.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.
// 

using System.Text.RegularExpressions;

namespace FFXIVAPP.Classes.RegExs
{
    internal static class Shared
    {
        public const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

        public static readonly Regex Romans = new Regex(@"(?<roman>\b[IVXLCDM]+\b)", DefaultOptions);

        public static readonly Regex Cicuid = new Regex(@".+=(?<cicuid>\d+)", DefaultOptions);

        public static readonly Regex Exp = new Regex(@"^You earn (?<exp>\d+).+\.$", DefaultOptions);

        public static readonly Regex ParseCOMS = new Regex(@"com:(?<cmd>(show-(mob|total)|parse)) ((?<cm>[\w\s]+):)?(?<sub>[\w\s-']+)( (?<limit>\d))?$", DefaultOptions);

        public static readonly Regex TranslateCOMS = new Regex(@"^/\w( \w+ \w+)?$", DefaultOptions);
    }
}