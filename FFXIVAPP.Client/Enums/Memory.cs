using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartAssembly.Attributes;

namespace FFXIVAPP.Client.Enums
{
    [DoNotObfuscate]
    public class Memory
    {
        public enum Job
        {
            GLD = 0x1,
            PGL = 0x2,
            MRD = 0x3,
            LNC = 0x4,
            ARC = 0x5,
            CNJ = 0x6,
            THM = 0x7,
            CPT = 0x8,
            BSM = 0x9,
            ARM = 0xA,
            GSM = 0xB,
            LTW = 0xC,
            WVR = 0xD,
            ALC = 0xE,
            CUL = 0xF,
            MIN = 0x10,
            BOT = 0x11,
            FSH = 0x12,
            PLD = 0x13,
            MNK = 0x14,
            WAR = 0x15,
            DRG = 0x16,
            BRD = 0x17,
            WHM = 0x18,
            BLM = 0x19,
            ACN = 0x2A,
            SMN = 0x2B,
            SCH = 0x2C,
            Chocobo = 0x2D,
            Pet = 0x2E
        }

        public class NPC
        {
            public enum Type
            {
                NPC = 0x01,
                PC = 0x02,
                Monster = 0x03,
                Gathering = 0x04
            }
            
            public enum CurrentTarget
            {
                Own = 0x1,
                True = 0x2,
                False = 0x4,
            }

            public enum Icon
            {
                None = 0x0,
                Yoshida = 0x1,
                GM = 0x2,
                SGM = 0x3,
                Clover = 0x4,
                DC = 0x5,
                Smiley = 0x6,
                Red_Cross = 0x9,
                Grey_DC = 0xA,
                Processing = 0xB,
                Busy = 0xC,
                Duty = 0xD,
                Processing_Yellow = 0xE,
                Processing_Grey = 0xF,
                Cutscene = 0x10,
                Chocobo = 0x12,
                Sitting = 0x13,
                Wrench_Yellow = 0x14,
                Wrench = 0x15,
                Dice = 0x16,
                Processing_Green = 0x17,
                Sword = 0x18,
                DutyFinder = 0x19,
                Alliance_Leader = 0x1A,
                Alliance_Blue_Leader = 0x1B,
                Alliance_Blue = 0x1C,
                Sprout = 0x1F,
                Gil = 0x20
            }

            public enum Sex
            {
                Male = 0x0,
                Female = 0x1
            }

            public enum Status
            {
                Idle = 0x01,
                Dead = 0x02,
                Sitting = 0x03,
                Mounted = 0x04,
                Crafting = 0x05,
                Gathering = 0x06,
                Melding = 0x07,
                SMachine = 0x08
            }

            public enum ClaimStatus
            {
                Engaged = 0x01,
                Idle = 0x02
            }
        }
    }
}
