// FFXIVAPP.Client
// NPCEntryHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Collections.Generic;
using FFXIVAPP.Client.Memory;
using Newtonsoft.Json;

namespace FFXIVAPP.Client.Helpers.SocketIO
{
    public static class NPCEntryHelper
    {
        #region Property Backings

        private static SocketIOClient.Client _socket;

        private static SocketIOClient.Client Socket
        {
            get { return _socket ?? (_socket = new SocketIOClient.Client("http://xivpads.com:843")); }
            set { _socket = value; }
        }

        #endregion

        #region Declarations

        public const int ChunkSize = 50;
        public static int ChunksProcessed = 0;
        public static bool Processing;

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="entries"></param>
        public static void ProcessUpload(List<NPCEntry> entries)
        {
            Processing = true;
            try
            {
                DestorySocket();
                if (Socket == null)
                {
                    Socket = new SocketIOClient.Client("http://xivpads.com:843");
                }
                Socket.Opened += delegate { };
                Socket.Message += delegate { };
                Socket.SocketConnectionClosed += delegate { DestorySocket(); };
                Socket.Error += delegate { DestorySocket(); };
                Socket.On("import_npc_success", delegate
                {
                    ChunksProcessed++;
                    DestorySocket();
                });
                Socket.On("import_npc_error", delegate { DestorySocket(); });
                Socket.On("connect", message => Socket.Emit("import_npc", JsonConvert.SerializeObject(entries)));
                Socket.Connect();
            }
            catch (Exception ex)
            {
            }
        }

        private static void DestorySocket()
        {
            try
            {
                if (Socket.IsConnected)
                {
                    Socket.Close();
                }
                Socket = null;
            }
            catch (Exception ex)
            {
            }
            Processing = false;
        }
    }
}
