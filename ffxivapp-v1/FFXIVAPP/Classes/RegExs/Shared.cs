// FFXIVAPP
// Shared.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using System.Text.RegularExpressions;

#endregion

namespace FFXIVAPP.Classes.RegExs
{
    public static class Shared
    {
        public const RegexOptions DefaultOptions = RegexOptions.Compiled | RegexOptions.ExplicitCapture;

        public static readonly Regex Romans = new Regex(@"(?<roman>\b[IVXLCDM]+\b)", DefaultOptions);

        public static readonly Regex Cicuid = new Regex(@".+=(?<cicuid>\d+)", DefaultOptions);

        public static readonly Regex Exp = new Regex(@"^You earn (?<exp>\d+).+\.$", DefaultOptions);

        public static readonly Regex ParseCOMS = new Regex(@"com:(?<cmd>(show-(mob|total)|parse)) ((?<cm>[\w\s]+):)?(?<sub>[\w\s-']+)( (?<limit>\d))?$", DefaultOptions);

        public static readonly Regex TranslateCOMS = new Regex(@"^/\w( \w+ \w+)?$", DefaultOptions);
    }
}
