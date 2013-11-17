// FFXIVAPP.Common
// Enums.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public class Enums
    {
        public enum Job
        {
            Unknown,
            GLD,
            PGL,
            MRD,
            LNC,
            ARC,
            CNJ,
            THM,
            CPT,
            BSM,
            ARM,
            GSM,
            LTW,
            WVR,
            ALC,
            CUL,
            MIN,
            BOT,
            FSH,
            PLD,
            MNK,
            WAR,
            DRG,
            BRD,
            WHM,
            BLM,
            ACN,
            SMN,
            SCH,
            Chocobo,
            Pet
        }

        public class Actor
        {
            public enum Claimed
            {
                Unknown,
                Claimed,
                UnClaimed
            }

            public enum Icon
            {
                Unknown,
                None,
                Yoshida,
                GM,
                SGM,
                Clover,
                DC,
                Smiley,
                Red_Cross,
                Grey_DC,
                Processing,
                Busy,
                Duty,
                Processing_Yellow,
                Processing_Grey,
                Cutscene,
                Chocobo,
                Sitting,
                Wrench_Yellow,
                Wrench,
                Dice,
                Processing_Green,
                Sword,
                DutyFinder,
                Alliance_Leader,
                Alliance_Blue_Leader,
                Alliance_Blue,
                Sprout,
                Gil
            }

            public enum Sex
            {
                Unknown,
                Male,
                Female
            }

            public enum Status
            {
                Unknown,
                Idle,
                Dead,
                Sitting,
                Mounted,
                Crafting,
                Gathering,
                Melding,
                SMachine
            }

            public enum TargetType
            {
                Unknown,
                Own,
                True,
                False,
            }

            public enum Type
            {
                Unknown,
                NPC,
                PC,
                Monster,
                Gathering
            }
        }
    }
}
