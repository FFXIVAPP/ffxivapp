// FFXIVAPP.Client
// NPCWorkerDelegate.cs
// 
// © 2013 Ryan Wilson

#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using FFXIVAPP.Client.Memory;
using Newtonsoft.Json;

#endregion

namespace FFXIVAPP.Client.Delegates
{
    internal static class NPCWorkerDelegate
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

        public static NPCEntry CurrentUser;
        public static readonly List<NPCEntry> MonsterList = new List<NPCEntry>();
        private static int _chunksProcessed = 0;
        private static int _chunkSize = 10;

        #endregion

        /// <summary>
        /// </summary>
        public static void OnNewNPC(List<NPCEntry> npcEntry)
        {
            if (!npcEntry.Any())
            {
                return;
            }
            CurrentUser = npcEntry.First();
            Func<bool> saveToDictionary = delegate
            {
                foreach (var entry in npcEntry.Where(e => e.NPCType == NPCType.Monster)
                                              .Where(entry => MonsterList.All(m => m.ID != entry.ID)))
                {
                    MonsterList.Add(entry);
                }
                return true;
            };
            saveToDictionary.BeginInvoke(delegate
            {
                if (MonsterList.Count > (_chunkSize * (_chunksProcessed + 1)))
                {
                    //ProcessUpload(new List<NPCEntry>(MonsterList.Skip(_chunksProcessed * _chunkSize)));
                }
            }, saveToDictionary);
        }

        /// <summary>
        /// </summary>
        /// <param name="entries"></param>
        private static void ProcessUpload(List<NPCEntry> entries)
        {
            DestorySocket();
            if (Socket == null)
            {
                Socket = new SocketIOClient.Client("http://xivpads.com:843");
            }
            try
            {
                Socket.Opened += delegate { };
                Socket.Message += delegate { };
                Socket.SocketConnectionClosed += delegate { DestorySocket(); };
                Socket.Error += delegate { DestorySocket(); };
                Socket.On("import_mob_success", delegate
                {
                    _chunksProcessed++;
                    DestorySocket();
                });
                Socket.On("import_mob_error", delegate { DestorySocket(); });
                Socket.On("connect", message => Socket.Emit("import_mob", JsonConvert.SerializeObject(entries)));
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
                Socket.Close();
                Socket = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
