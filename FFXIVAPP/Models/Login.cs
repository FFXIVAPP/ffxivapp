// FFXIVAPP
// Login.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using Newtonsoft.Json;

namespace FFXIVAPP.Models
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