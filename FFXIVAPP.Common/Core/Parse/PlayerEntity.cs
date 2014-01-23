// FFXIVAPP.Common
// PlayerEntity.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Text.RegularExpressions;
using FFXIVAPP.Common.Core.Memory.Enums;
using FFXIVAPP.Common.Core.Parse.Enums;
using FFXIVAPP.Common.Core.Parse.Interfaces;
using FFXIVAPP.Common.Helpers;

namespace FFXIVAPP.Common.Core.Parse
{
    public class PlayerEntity : IPlayerEntity
    {
        private string _name;

        public string FirstName
        {
            get
            {
                try
                {
                    return Name.Split(' ')[0];
                }
                catch (Exception ex)
                {
                    return Name;
                }
            }
        }

        public string LastName
        {
            get
            {
                try
                {
                    return Name.Split(' ')[1];
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
        }

        public string NameInitialsOnly
        {
            get
            {
                var missingLastName = String.IsNullOrWhiteSpace(LastName);
                try
                {
                    if (missingLastName)
                    {
                        return String.Format("{0}.", FirstName.Substring(0, 1));
                    }
                    return String.Format("{0}.{1}.", FirstName.Substring(0, 1), LastName.Substring(0, 1));
                }
                catch (Exception ex)
                {
                    return Name;
                }
            }
        }

        public PlayerType Type { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = StringHelper.TitleCase(Regex.Replace(value, @"\[[\w]+\]", "")
                                                    .Trim());
            }
        }

        public Actor.Job Job { get; set; }
        public decimal CombinedDPS { get; set; }
        public decimal DPS { get; set; }
        public decimal DOTPS { get; set; }
        public decimal CombinedHPS { get; set; }
        public decimal HPS { get; set; }
        public decimal HOTPS { get; set; }
        public decimal HOHPS { get; set; }
        public decimal HMPS { get; set; }
        public decimal CombinedDTPS { get; set; }
        public decimal DTPS { get; set; }
        public decimal DTOTPS { get; set; }
        public decimal CombinedTotalOverallDamage { get; set; }
        public decimal TotalOverallDamage { get; set; }
        public decimal TotalOverallDamageOverTime { get; set; }
        public decimal CombinedTotalOverallHealing { get; set; }
        public decimal TotalOverallHealing { get; set; }
        public decimal TotalOverallHealingOverTime { get; set; }
        public decimal TotalOverallHealingOverHealing { get; set; }
        public decimal TotalOverallHealingMitigated { get; set; }
        public decimal CombinedTotalOverallDamageTaken { get; set; }
        public decimal TotalOverallDamageTaken { get; set; }
        public decimal TotalOverallDamageTakenOverTime { get; set; }
        public decimal PercentOfTotalOverallDamage { get; set; }
        public decimal PercentOfTotalOverallDamageOverTime { get; set; }
        public decimal PercentOfTotalOverallHealing { get; set; }
        public decimal PercentOfTotalOverallHealingOverTime { get; set; }
        public decimal PercentOfTotalOverallHealingOverHealing { get; set; }
        public decimal PercentOfTotalOverallHealingMitigated { get; set; }
        public decimal PercentOfTotalOverallDamageTaken { get; set; }
        public decimal PercentOfTotalOverallDamageTakenOverTime { get; set; }
    }
}
