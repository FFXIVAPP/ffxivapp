// FFXIVAPP.Client
// PlayerInfoWorker.cs
// 
// Copyright © 2007 - 2014 Ryan Wilson - All Rights Reserved
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met: 
// 
//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright 
//    notice, this list of conditions and the following disclaimer in the 
//    documentation and/or other materials provided with the distribution. 
//  * Neither the name of SyndicatedLife nor the names of its contributors may 
//    be used to endorse or promote products derived from this software 
//    without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Timers;
using FFXIVAPP.Client.Helpers;
using FFXIVAPP.Client.Properties;
using FFXIVAPP.Common.Core.Memory;
using FFXIVAPP.Common.Core.Memory.Enums;
using Newtonsoft.Json;
using NLog;

namespace FFXIVAPP.Client.Memory
{
    internal class PlayerInfoWorker : INotifyPropertyChanged, IDisposable
    {
        #region Logger

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Property Bindings

        private uint PlayerInfoMap { get; set; }

        private PlayerEntity LastPlayerEntity { get; set; }

        #endregion

        #region Declarations

        private readonly Timer _scanTimer;
        private bool _isScanning;

        #endregion

        public PlayerInfoWorker()
        {
            _scanTimer = new Timer(1000);
            _scanTimer.Elapsed += ScanTimerElapsed;
        }

        #region Timer Controls

        /// <summary>
        /// </summary>
        public void StartScanning()
        {
            _scanTimer.Enabled = true;
        }

        /// <summary>
        /// </summary>
        public void StopScanning()
        {
            _scanTimer.Enabled = false;
        }

        #endregion

        #region Threads

        /// <summary>
        /// </summary>
        /// <param name="sender"> </param>
        /// <param name="e"> </param>
        private void ScanTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_isScanning)
            {
                return;
            }
            _isScanning = true;
            double refresh = 1000;
            if (Double.TryParse(Settings.Default.PlayerInfoWorkerRefresh.ToString(CultureInfo.InvariantCulture), out refresh))
            {
                _scanTimer.Interval = refresh;
            }
            Func<bool> scannerWorker = delegate
            {
                if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("CHARMAP"))
                {
                    if (MemoryHandler.Instance.SigScanner.Locations.ContainsKey("PLAYERINFO"))
                    {
                        PlayerInfoMap = MemoryHandler.Instance.SigScanner.Locations["PLAYERINFO"];
                        if (PlayerInfoMap <= 6496)
                        {
                            return false;
                        }
                        try
                        {
                            var enmityCount = MemoryHandler.Instance.GetInt16(MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 5688);
                            var enmityStructure = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 3384;
                            var enmityEntries = new List<EnmityEntry>();
                            if (enmityCount > 0 && enmityCount < 32 && enmityStructure > 0)
                            {
                                for (uint i = 0; i < enmityCount; i++)
                                {
                                    var address = enmityStructure + (i * 72);
                                    var enmityEntry = new EnmityEntry
                                    {
                                        Name = MemoryHandler.Instance.GetString(address),
                                        ID = (uint) MemoryHandler.Instance.GetInt32(address + 64),
                                        Enmity = (uint) MemoryHandler.Instance.GetInt16(address + 68)
                                    };
                                    if (enmityEntry.ID > 0)
                                    {
                                        enmityEntries.Add(enmityEntry);
                                    }
                                }
                            }
                            var playerInfo = MemoryHandler.Instance.GetStructure<Structures.PlayerInfo>(PlayerInfoMap);
                            var playerEntity = new PlayerEntity
                            {
                                Name = MemoryHandler.Instance.GetString(PlayerInfoMap, 1),
                                EnmityEntries = enmityEntries,
                                Accuracy = playerInfo.Accuracy,
                                ACN = playerInfo.ACN,
                                ACN_CurrentEXP = playerInfo.ACN_CurrentEXP,
                                ALC = playerInfo.ALC,
                                ARC = playerInfo.ARC,
                                ARC_CurrentEXP = playerInfo.ARC_CurrentEXP,
                                ARM = playerInfo.ARM,
                                ARM_CurrentEXP = playerInfo.ARM_CurrentEXP,
                                AttackMagicPotency = playerInfo.AttackMagicPotency,
                                AttackPower = playerInfo.AttackPower,
                                BTN = playerInfo.BTN,
                                BTN_CurrentEXP = playerInfo.BTN_CurrentEXP,
                                BSM = playerInfo.BSM,
                                BSM_CurrentEXP = playerInfo.BSM_CurrentEXP,
                                BaseDexterity = playerInfo.BaseDexterity,
                                BaseIntelligence = playerInfo.BaseIntelligence,
                                BaseMind = playerInfo.BaseMind,
                                BasePiety = playerInfo.BasePiety,
                                BaseStrength = playerInfo.BaseStrength,
                                BaseVitality = playerInfo.BaseVitality,
                                CNJ = playerInfo.CNJ,
                                CNJ_CurrentEXP = playerInfo.CNJ_CurrentEXP,
                                CPMax = playerInfo.CPMax,
                                CPT = playerInfo.CPT,
                                CPT_CurrentEXP = playerInfo.CPT_CurrentEXP,
                                CUL = playerInfo.CUL,
                                CUL_CurrentEXP = playerInfo.CUL_CurrentEXP,
                                Control = playerInfo.Control,
                                Craftmanship = playerInfo.Craftmanship,
                                CriticalHitRate = playerInfo.CriticalHitRate,
                                Defense = playerInfo.Defense,
                                Determination = playerInfo.Determination,
                                Dexterity = playerInfo.Dexterity,
                                FSH = playerInfo.FSH,
                                FSH_CurrentEXP = playerInfo.FSH_CurrentEXP,
                                FireResistance = playerInfo.FireResistance,
                                GLD = playerInfo.GLD,
                                GLD_CurrentEXP = playerInfo.GLD_CurrentEXP,
                                GPMax = playerInfo.GPMax,
                                GSM = playerInfo.GSM,
                                GSM_CurrentEXP = playerInfo.GSM_CurrentEXP,
                                Gathering = playerInfo.Gathering,
                                HPMax = playerInfo.HPMax,
                                HealingMagicPotency = playerInfo.HealingMagicPotency,
                                IceResistance = playerInfo.IceResistance,
                                Intelligence = playerInfo.Intelligence,
                                JobID = playerInfo.JobID,
                                LNC = playerInfo.LNC,
                                LNC_CurrentEXP = playerInfo.LNC_CurrentEXP,
                                LTW = playerInfo.LTW,
                                LTW_CurrentEXP = playerInfo.LTW_CurrentEXP,
                                LightningResistance = playerInfo.LightningResistance,
                                MIN = playerInfo.MIN,
                                MIN_CurrentEXP = playerInfo.MIN_CurrentEXP,
                                MPMax = playerInfo.MPMax,
                                MRD = playerInfo.MRD,
                                MRD_CurrentEXP = playerInfo.MRD_CurrentEXP,
                                MagicDefense = playerInfo.MagicDefense,
                                Mind = playerInfo.Mind,
                                PGL = playerInfo.PGL,
                                PGL_CurrentEXP = playerInfo.PGL_CurrentEXP,
                                Parry = playerInfo.Parry,
                                Perception = playerInfo.Perception,
                                PiercingResistance = playerInfo.PiercingResistance,
                                Piety = playerInfo.Piety,
                                SkillSpeed = playerInfo.SkillSpeed,
                                SlashingResistance = playerInfo.SlashingResistance,
                                SpellSpeed = playerInfo.SpellSpeed,
                                Strength = playerInfo.Strength,
                                THM = playerInfo.THM,
                                THM_CurrentEXP = playerInfo.THM_CurrentEXP,
                                TPMax = playerInfo.TPMax,
                                Vitality = playerInfo.Vitality,
                                WVR = playerInfo.WVR,
                                WVR_CurrentEXP = playerInfo.WVR_CurrentEXP,
                                WaterResistance = playerInfo.WaterResistance,
                                WindResistance = playerInfo.WindResistance
                            };
                            playerEntity.Job = (Actor.Job) playerEntity.JobID;
                            var notify = false;
                            if (LastPlayerEntity == null)
                            {
                                LastPlayerEntity = playerEntity;
                                notify = true;
                            }
                            else
                            {
                                var hash1 = JsonConvert.SerializeObject(LastPlayerEntity)
                                                       .GetHashCode();
                                var hash2 = JsonConvert.SerializeObject(playerEntity)
                                                       .GetHashCode();
                                if (!hash1.Equals(hash2))
                                {
                                    LastPlayerEntity = playerEntity;
                                    notify = true;
                                }
                            }
                            if (notify)
                            {
                                AppContextHelper.Instance.RaiseNewPlayerEntity(playerEntity);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            PlayerInfoMap = MemoryHandler.Instance.SigScanner.Locations["CHARMAP"] + 5724;
                            MemoryHandler.Instance.SigScanner.Locations.Add("PLAYERINFO", PlayerInfoMap);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                _isScanning = false;
                return true;
            };
            scannerWorker.BeginInvoke(delegate { }, scannerWorker);
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _scanTimer.Elapsed -= ScanTimerElapsed;
        }

        #endregion
    }
}
