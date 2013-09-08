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
        public static NPCEntry CurrentUser;
        public static readonly List<NPCEntry> MonsterList = new List<NPCEntry>();
        private static SocketIOClient.Client _socket;
        private static int _chunksProcessed = 0;
        private static int _chunkSize = 10;

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
            if (_socket == null)
            {
                _socket = new SocketIOClient.Client("http://xivpads.com:843");
                _socket.Opened += delegate { };
                _socket.Message += delegate { };
                _socket.SocketConnectionClosed += delegate { _socket = null; };
                _socket.Error += delegate { _socket = null; };
                _socket.On("import_mob_success", delegate
                {
                    _chunksProcessed++;
                    _socket = null;
                });
                _socket.On("import_mob_error", delegate { _socket = null; });
                _socket.Connect();
            }
            if (_socket.IsConnected)
            {
                _socket.Emit("import_mob", JsonConvert.SerializeObject(entries));
                return;
            }
            try
            {
                _socket.Connect();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
