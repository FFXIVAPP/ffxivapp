// FFXIVAPP.Client
// Filter.Declarations.cs
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

using FFXIVAPP.Client.Models.Parse.Events;

namespace FFXIVAPP.Client.Utilities.Parse
{
    public static partial class Filter
    {
        // setup you
        private static Event _lastEventYou;
        private static string _lastActionYou = "";
        private static bool _lastActionYouIsAttack;

        // setup you pet
        private static Event _lastEventPet;
        private static string _lastNamePet = "";
        private static string _lastActionPet = "";
        private static bool _lastActionPetIsAttack;

        // setup party info
        private static Event _lastEventParty;
        private static string _lastNamePartyFrom = "";
        private static string _lastActionPartyFrom = "";
        private static string _lastNamePartyHealingFrom = "";
        private static string _lastActionPartyHealingFrom = "";
        private static string _lastNamePartyTo = "";
        private static bool _lastActionPartyIsAttack;

        // setup party pet info
        private static Event _lastEventPetParty;
        private static string _lastNamePetPartyFrom = "";
        private static string _lastActionPetPartyFrom = "";
        private static string _lastNamePetPartyHealingFrom = "";
        private static string _lastActionPetPartyHealingFrom = "";
        private static string _lastNamePetPartyTo = "";
        private static bool _lastActionPetPartyIsAttack;

        // setup alliance info
        private static Event _lastEventAlliance;
        private static string _lastNameAllianceFrom = "";
        private static string _lastActionAllianceFrom = "";
        private static string _lastNameAllianceHealingFrom = "";
        private static string _lastActionAllianceHealingFrom = "";
        private static string _lastNameAllianceTo = "";
        private static bool _lastActionAllianceIsAttack;

        // setup alliancepet  info
        private static Event _lastEventPetAlliance;
        private static string _lastNamePetAllianceFrom = "";
        private static string _lastActionPetAllianceFrom = "";
        private static string _lastNamePetAllianceHealingFrom = "";
        private static string _lastActionPetAllianceHealingFrom = "";
        private static string _lastNamePetAllianceTo = "";
        private static bool _lastActionPetAllianceIsAttack;

        // setup other info
        private static Event _lastEventOther;
        private static string _lastNameOtherFrom = "";
        private static string _lastActionOtherFrom = "";
        private static string _lastNameOtherHealingFrom = "";
        private static string _lastActionOtherHealingFrom = "";
        private static string _lastNameOtherTo = "";
        private static bool _lastActionOtherIsAttack;

        // setup otherpet  info
        private static Event _lastEventPetOther;
        private static string _lastNamePetOtherFrom = "";
        private static string _lastActionPetOtherFrom = "";
        private static string _lastNamePetOtherHealingFrom = "";
        private static string _lastActionPetOtherHealingFrom = "";
        private static string _lastNamePetOtherTo = "";
        private static bool _lastActionPetOtherIsAttack;

        // setup monster info
        private static string _lastNameMonster = "";
        private static string _lastActionMonster = "";
        private static bool _lastActionMonsterIsAttack;
    }
}
