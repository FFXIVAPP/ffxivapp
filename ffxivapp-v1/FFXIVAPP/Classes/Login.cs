// FFXIVAPP
// Login.cs
//  
// Created by Ryan Wilson.
// Copyright © 2007-2012 Ryan Wilson - All Rights Reserved

#region Usings

using Newtonsoft.Json;

#endregion

namespace FFXIVAPP.Classes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Login
    {
        [JsonProperty]
        public string source { get; set; }

        [JsonProperty]
        public string type { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public string api { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Login Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<Login>(jsonString);
        }
    }
}
