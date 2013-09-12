// FFXIVAPP.Client
// Client.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Memory;
using FFXIVAPP.Client.Models;
using Newtonsoft.Json;

namespace FFXIVAPP.Client.Helpers.SocketIO
{
    public class Client : SocketIOClient.Client
    {
        #region Auto Properties

        private SocketIOClient.Client SocketClient { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="socketKey"></param>
        /// <param name="entries"></param>
        public Client(string socketKey, object entries) : base(socketKey)
        {
            try
            {
                object data;
                switch (socketKey)
                {
                    case "import_npc":
                        data = entries as IList<NPCEntry>;
                        break;
                    case "import_mob":
                        data = entries as IList<NPCEntry>;
                        break;
                    case "import_kill":
                        data = entries as IList<KillEntry>;
                        break;
                    case "import_loot":
                        data = entries as IList<LootEntry>;
                        break;
                    default:
                        data = entries;
                        break;
                }
                SocketClient = new SocketIOClient.Client("http://xivpads.com:843")
                {
                    RetryConnectionAttempts = 0
                };
                SocketClient.Opened += delegate { };
                SocketClient.Message += delegate { };
                SocketClient.SocketConnectionClosed += delegate { DestorySocketClient(); };
                SocketClient.Error += delegate { DestorySocketClient(); };
                SocketClient.On(String.Format("{0}_success", socketKey), delegate { DestorySocketClient(); });
                SocketClient.On(String.Format("{0}_error", socketKey), delegate { DestorySocketClient(); });
                SocketClient.On("connect", delegate
                {
                    SocketClient.Emit("welcome", "{\"message\":\"hello[" + socketKey + "]\"}");
                    SocketClient.Emit(socketKey, JsonConvert.SerializeObject(data));
                });
                SocketClient.Connect();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// </summary>
        private void DestorySocketClient()
        {
            try
            {
                SocketClient.Close();
                SocketClient.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
