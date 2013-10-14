// FFXIVAPP.Client
// Position.cs
// 
// © 2013 Ryan Wilson

using System;

namespace FFXIVAPP.Client.Plugins.Parse.Models {
    public class Position {
        public Position(int x = 0, int y = 0) {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString() {
            return String.Format("{0},{1}", X, Y);
        }
    }
}
