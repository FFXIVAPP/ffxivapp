// Project: ChatModXIV
// File: Login.cs
// 
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using System.Reflection;
using Newtonsoft.Json;

namespace ChatModXIV.Classes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Login
    {
        [JsonProperty]
        public string Source { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Api { get; set; }

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