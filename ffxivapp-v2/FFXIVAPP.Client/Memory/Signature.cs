// FFXIVAPP.Client
// Signature.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2013 Ryan Wilson - All Rights Reserved

#region Usings

using System.Text.RegularExpressions;

#endregion

namespace FFXIVAPP.Client.Memory
{
    internal class Signature
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Regex RegularExpress { get; set; }
        public int Offset { get; set; }
    }
}
