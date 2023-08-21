using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_sync_win.models
{
    internal class FileWatcherSettings
    {
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }

        public FileWatcherSettings() { }

        public FileWatcherSettings(string filePath, string fileType, string serverAddress, int serverPort)
        {
            FilePath = filePath;
            FileType = fileType;
            ServerAddress = serverAddress;
            ServerPort = serverPort;
        }

        public void GetSettings()
        {
            FilePath = Environment.GetEnvironmentVariable("FSW_FILEPATH");
            FileType = Environment.GetEnvironmentVariable("FSW_FILETYPE");
            ServerAddress = Environment.GetEnvironmentVariable("FSW_SERVER_ADDRESS");
            // todo: ServerPort should be included in ServerAddress OR set separately
        }

        public override string ToString()
        {
            return $"Server: {ServerAddress} FilePath: {FilePath} Filter: {FileType}";
        }
    }
}
