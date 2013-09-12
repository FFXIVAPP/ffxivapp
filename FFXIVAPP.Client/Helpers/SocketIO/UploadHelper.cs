// FFXIVAPP.Client
// UploadHelper.cs
// 
// © 2013 Ryan Wilson

using FFXIVAPP.Client.Properties;

namespace FFXIVAPP.Client.Helpers.SocketIO
{
    public class UploadHelper
    {
        #region Auto-Properties

        public int ChunkSize { get; set; }
        public int ChunksProcessed { get; set; }

        #endregion

        /// <summary>
        /// </summary>
        /// <param name="chunkSize"></param>
        public UploadHelper(int chunkSize = 200)
        {
            ChunkSize = chunkSize;
            ChunksProcessed = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="socketKey"></param>
        /// <param name="entries"></param>
        public void ProcessUpload(string socketKey, object entries)
        {
            if (Settings.Default.AllowXIVDBIntegration)
            {
                var client = new Client(socketKey, entries);
            }
        }
    }
}
