// FFXIVAPP.Client ~ WindowsMessageEvents.cs
// 
// Copyright © 2007 - 2015 Ryan Wilson - All Rights Reserved
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace FFXIVAPP.Client.Models
{
    public static class WindowsMessageEvents
    {
        public const int KeyDown = 0x100;
        public const int KeyUp = 0x101;
        public const int Char = 0x102;
        public const int UniChar = 0x109;
        public const int Paste = 0x302;
        public const int SetText = 0x0C;
    }
}
