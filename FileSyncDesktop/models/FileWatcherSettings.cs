using System;

namespace FileSyncDesktop.Models
{
    public class FileWatcherSettings : IFileWatcherSettings
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

        public bool HasFileSettings()
        {
            return FilePath != string.Empty && FileType.Substring(0, 2) == "*.";
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

        public bool HasServerSettings()
        {
            return ServerAddress != string.Empty && ServerPort != string.Empty;
        }

        public override string ToString()
        {
            return $"Server: {ServerAddress} FilePath: {FilePath} Filter: {FileType}";
        }
    }
}
