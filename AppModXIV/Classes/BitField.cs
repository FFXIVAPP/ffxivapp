// AppModXIV
// BitField.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System;

namespace AppModXIV.Classes
{
    public class BitField
    {
        /// <summary>
        /// Hexidecimal		Decimal		Binary
        /// 0x...0000		0		    00000000000000000
        /// 0x...0001		1	    	00000000000000001
        /// 0x...0002		2	    	00000000000000010
        /// 0x...0004		4	    	00000000000000100
        /// 0x...0008		8	    	00000000000001000
        /// 0x...0010		16	    	00000000000010000
        /// 0x...0020		32	    	00000000000100000
        /// 0x...0040		64	    	00000000001000000
        /// 0x...0080		128	    	00000000010000000
        /// 0x...0100		256	    	00000000100000000
        /// 0x...0200		512	    	00000001000000000
        /// 0x...0400		1024		00000010000000000
        /// 0x...0800		2048		00000100000000000
        /// 0x...1000		4096		00001000000000000
        /// 0x...2000		8192		00010000000000000
        /// 0x...4000		16384		00100000000000000
        /// 0x...8000		32768		01000000000000000
        /// </summary>
        [Flags]
        public enum Flag : ulong
        {
            // Hexidecimal		Decimal		Binary
            Clear = 0x00,
            F1 = 0x01,
            F2 = F1 << 1,
            F3 = F2 << 1,
            F4 = F3 << 1,
            F5 = F4 << 1,
            F6 = F5 << 1,
            F7 = F6 << 1,
            F8 = F7 << 1,
            F9 = F8 << 1,
            F10 = F9 << 1,
            F11 = F10 << 1,
            F12 = F11 << 1,
            F13 = F12 << 1,
            F14 = F13 << 1,
            F15 = F14 << 1,
            F16 = F15 << 1
        };

        /// <summary>
        /// 
        /// </summary>
        public ulong Mask { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flg"></param>
        public BitField(Flag flg)
        {
            Mask = (ulong) flg;
        }

        /// <summary>
        /// ClearField clears all contents of the Field
        /// </summary>
        public void ClearField()
        {
            SetField(Flag.Clear);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flg"></param>
        private void SetField(Flag flg)
        {
            Mask = (ulong) flg;
        }

        /// <summary>
        /// Setting the specified flag and leaving all other flags unchanged
        /// </summary>
        /// <param name="flg"></param>
        public void SetOn(Flag flg)
        {
            Mask |= (ulong) flg;
        }

        /// <summary>
        /// Unsetting the specified flag and leaving all other flags unchanged
        /// </summary>
        /// <param name="flg"></param>
        public void SetOff(Flag flg)
        {
            Mask &= ~(ulong) flg;
        }

        /// <summary>
        /// Toggling the specified flag and leaving all other bits unchanged
        /// </summary>
        /// <param name="flg"></param>
        public void SetToggle(Flag flg)
        {
            Mask ^= (ulong) flg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flg"></param>
        /// <returns></returns>
        public bool AllOn(Flag flg)
        {
            return (Mask & (ulong) flg) == (ulong) flg;
        }

        /// <summary>
        /// Convert a decimal value to a Flag FlagsAttribute value
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static Flag DecimalToFlag(decimal dec)
        {
            ulong tMsk = 0;

            var shift = (byte) dec;
            if (shift > 0 && shift <= 64)
            {
                tMsk = (ulong) 0x01 << (shift - 1);
            }

            return (Flag) tMsk;
        }

        /// <summary>
        /// Return a string representation of the Field in decimal (base 10) notation
        /// </summary>
        /// <returns></returns>
        public String ToStringDec()
        {
            return String.Format("{0}", Mask);
        }
    }
}