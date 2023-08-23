using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncDesktop.Models
{
    public class FileWatcherSettings
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string ServerAddress { get; set; } = string.Empty;
        public string ServerPort { get; set; } = string.Empty;

        public FileWatcherSettings() { }

        public FileWatcherSettings(string filePath, string fileType, string serverAddress, string serverPort)
        {
            FilePath = filePath;
            FileType = fileType;
            ServerAddress = serverAddress;
            ServerPort = serverPort;
        }

        public void GetSettingsFromEnvVars()
        {
            FilePath = Environment.GetEnvironmentVariable("FSW_FILEPATH");
            FileType = Environment.GetEnvironmentVariable("FSW_FILETYPE");
            ServerAddress = Environment.GetEnvironmentVariable("FSW_SERVERADDR");
            ServerPort = Environment.GetEnvironmentVariable("FSW_SERVERPORT");
        }

        public void RemoveFileSettings()
        {
            FilePath = string.Empty;
            FileType = string.Empty;
        }

        public void RemoveServerSettings()
        {
            ServerAddress = string.Empty;
            ServerPort = string.Empty;
        }

        public override string ToString()
        {
            return $"Server: {ServerAddress} FilePath: {FilePath} Filter: {FileType}";
        }
    }
}
