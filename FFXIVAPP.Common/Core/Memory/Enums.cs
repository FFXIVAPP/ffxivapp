// FFXIVAPP.Common
// Enums.cs
// 
// © 2013 Ryan Wilson

namespace FFXIVAPP.Common.Core.Memory
{
    public class Enums
    {
        public enum Job : byte
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
            public enum Claimed : byte
            {
                Unknown,
                Claimed,
                Idle,
                Crafting
            }

            public enum Icon : byte
            {
                Unknown,
                None,
                Yoshida,
                GM,
                SGM,
                Clover,
                DC,
                Smiley,
                RedCross,
                GreyDC,
                Processing,
                Busy,
                Duty,
                ProcessingYellow,
                ProcessingGrey,
                Cutscene,
                Chocobo,
                Sitting,
                WrenchYellow,
                Wrench,
                Dice,
                ProcessingGreen,
                Sword,
                DutyFinder,
                AllianceLeader,
                AllianceBlueLeader,
                AllianceBlue,
                Sprout,
                Gil
            }

            public enum Sex : byte
            {
                Unknown,
                Male,
                Female
            }

            public enum Status : byte
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

            public enum TargetType : byte
            {
                Unknown,
                Own,
                True,
                False,
            }

            public enum Type : byte
            {
                Unknown,
                PC,
                Monster,
                NPC,
                Aetheryte,
                Gathering,
                Minion
            }
        }
    }
}
