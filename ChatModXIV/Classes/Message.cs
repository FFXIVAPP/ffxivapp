// ChatModXIV
// Message.cs
//  
// Created by Ryan Wilson.
// Copyright (c) 2010-2012, Ryan Wilson. All rights reserved.

using Newtonsoft.Json;

namespace ChatModXIV.Classes
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Message
    {
        [JsonProperty]
        public string source { get; set; }

        [JsonProperty]
        public string type { get; set; }

        [JsonProperty]
        public string command { get; set; }

        [JsonProperty]
        public string server { get; set; }

        [JsonProperty]
        public string time { get; set; }

        [JsonProperty]
        public string code { get; set; }

        [JsonProperty]
        public string player { get; set; }

        [JsonProperty]
        public string message { get; set; }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Message Deserialize(string jsonString)
        {
            return JsonConvert.DeserializeObject<Message>(jsonString);
        }
    }
}