// FFXIVAPP.Common
// PlayerEntity.cs
// 
// © 2013 Ryan Wilson

using System.Collections.Generic;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Core.Memory.Interfaces;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Memory
{
    public class PlayerEntity : IPlayerEntity
    {
        private List<EnmityEntry> _enmityEntries;
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = StringHelper.TitleCase(value); }
        }

        public List<EnmityEntry> EnmityEntries
        {
            get { return _enmityEntries ?? (_enmityEntries = new List<EnmityEntry>()); }
            set
            {
                if (_enmityEntries == null)
                {
                    _enmityEntries = new List<EnmityEntry>();
                }
                _enmityEntries = value;
            }
        }

        public byte JobID { get; set; }
        public Actor.Job Job { get; set; }
        public byte PGL { get; set; }
        public byte GLD { get; set; }
        public byte MRD { get; set; }
        public byte ARC { get; set; }
        public byte LNC { get; set; }
        public byte THM { get; set; }
        public byte CNJ { get; set; }
        public byte ACN { get; set; }
        public byte CPT { get; set; }
        public byte BSM { get; set; }
        public byte ARM { get; set; }
        public byte GSM { get; set; }
        public byte LTW { get; set; }
        public byte WVR { get; set; }
        public byte ALC { get; set; }
        public byte CUL { get; set; }
        public byte MIN { get; set; }
        public byte BTN { get; set; }
        public byte FSH { get; set; }
        public int PGL_CurrentEXP { get; set; }
        public int GLD_CurrentEXP { get; set; }
        public int MRD_CurrentEXP { get; set; }
        public int ARC_CurrentEXP { get; set; }
        public int LNC_CurrentEXP { get; set; }
        public int THM_CurrentEXP { get; set; }
        public int CNJ_CurrentEXP { get; set; }
        public int ACN_CurrentEXP { get; set; }
        public int BSM_CurrentEXP { get; set; }
        public int CPT_CurrentEXP { get; set; }
        public int GSM_CurrentEXP { get; set; }
        public int ARM_CurrentEXP { get; set; }
        public int WVR_CurrentEXP { get; set; }
        public int LTW_CurrentEXP { get; set; }
        public int CUL_CurrentEXP { get; set; }
        public int MIN_CurrentEXP { get; set; }
        public int BTN_CurrentEXP { get; set; }
        public int FSH_CurrentEXP { get; set; }
        public short BaseStrength { get; set; }
        public short BaseDexterity { get; set; }
        public short BaseVitality { get; set; }
        public short BaseIntelligence { get; set; }
        public short BaseMind { get; set; }
        public short BasePiety { get; set; }
        public short Strength { get; set; }
        public short Dexterity { get; set; }
        public short Vitality { get; set; }
        public short Intelligence { get; set; }
        public short Mind { get; set; }
        public short Piety { get; set; }
        public int HPMax { get; set; }
        public int MPMax { get; set; }
        public int TPMax { get; set; }
        public int GPMax { get; set; }
        public int CPMax { get; set; }
        public short Parry { get; set; }
        public short Defense { get; set; }
        public short Evasion { get; set; }
        public short MagicDefense { get; set; }
        public short SlashingResistance { get; set; }
        public short PiercingResistance { get; set; }
        public short BluntResistance { get; set; }
        public short FireResistance { get; set; }
        public short IceResistance { get; set; }
        public short WindResistance { get; set; }
        public short EarthResistance { get; set; }
        public short LightningResistance { get; set; }
        public short WaterResistance { get; set; }
        public short AttackPower { get; set; }
        public short Accuracy { get; set; }
        public short CriticalHitRate { get; set; }
        public short AttackMagicPotency { get; set; }
        public short HealingMagicPotency { get; set; }
        public short Determination { get; set; }
        public short SkillSpeed { get; set; }
        public short SpellSpeed { get; set; }
        public short Craftmanship { get; set; }
        public short Control { get; set; }
        public short Gathering { get; set; }
        public short Perception { get; set; }
    }
}
