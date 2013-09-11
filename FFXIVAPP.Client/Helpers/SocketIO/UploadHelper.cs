// FFXIVAPP.Client
// UploadHelper.cs
// 
// © 2013 Ryan Wilson

using System;
using System.Windows;
using FFXIVAPP.Client.Properties;
using Newtonsoft.Json;

namespace FFXIVAPP.Client.Helpers.SocketIO
{
    public class UploadHelper
    {
        #region Property Backings

        private SocketIOClient.Client _socket;

        private SocketIOClient.Client Socket
        {
            get { return _socket ?? (_socket = new SocketIOClient.Client(ServiceUri)); }
            set { _socket = value; }
        }

        #endregion

        #region Auto-Properties

        public int ChunkSize { get; set; }
        public int ChunksProcessed { get; set; }
        public bool Processing { get; set; }

        #endregion

        #region Declarations

        private readonly string _socketEventKey = "";
        public string ServiceUri = "http://xivpads.com:843";

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="chunkSize"></param>
        public UploadHelper(string key, int chunkSize = 200)
        {
            _socketEventKey = key;
            ChunkSize = chunkSize;
            ChunksProcessed = 0;
            Processing = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="entries"></param>
        public void ProcessUpload(object entries)
        {
            if (Settings.Default.AllowXIVDBIntegration)
            {
                Processing = true;
                try
                {
                    DestorySocket();
                    if (Socket == null)
                    {
                        Socket = new SocketIOClient.Client(ServiceUri);
                    }
                    Socket.Opened += delegate { };
                    Socket.Message += delegate { };
                    Socket.SocketConnectionClosed += delegate { DestorySocket(); };
                    Socket.Error += delegate { DestorySocket(); };
                    Socket.On(String.Format("{0}_success", _socketEventKey), delegate
                    {
                        ChunksProcessed++;
                        DestorySocket();
                    });
                    Socket.On(String.Format("{0}_error", _socketEventKey), delegate { DestorySocket(); });
                    Socket.On("connect", message => Socket.Emit(_socketEventKey, JsonConvert.SerializeObject(entries)));
                    Socket.Connect();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                Processing = false;
            }
        }

        /// <summary>
        /// </summary>
        private void DestorySocket()
        {
            try
            {
                Socket.Close();
                Socket = null;
            }
            catch (Exception ex)
            {
            }
            Processing = false;
        }
    }
}
