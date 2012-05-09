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
        public string Source { get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string Command { get; set; }

        [JsonProperty]
        public string Server { get; set; }

        [JsonProperty]
        public string Time { get; set; }

        [JsonProperty]
        public string Code { get; set; }

        [JsonProperty]
        public string Player { get; set; }

        [JsonProperty]
        public string Data { get; set; }

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