using System;
using System.IO;
using System.Text.RegularExpressions;

namespace FileSyncDesktop.Library.Helpers
{
    public class FileWatcherSettings : IFileWatcherSettings
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string ServerAddress { get; set; } = string.Empty;
        public string ServerPort { get; set; } = string.Empty;
        private static readonly Regex _filterRegex = new Regex(@"^\*\.\w{0,4}$");
        private static readonly int _minServerPort = 5000;
        private static readonly int _maxServerPort = 65535;

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
            return FileSourcePathIsValid(FilePath) && FilterArgumentMatchesPattern(FileType);
        }

        public void SetFileSettings(string filePath, string fileType)
        {
            FilePath = filePath;
            FileType = fileType;
        }

        public void SetServerSettings(string serverAddress, string serverPort)
        {
            ServerAddress = serverAddress;
            ServerPort = serverPort;
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
            // todo: check for valid server address if not null or empty
            return ServerPortInValidRange(ServerPort) && !string.IsNullOrEmpty(ServerAddress);
        }

        public bool FileSourcePathIsValid(string value)
        {
            return Directory.Exists(value);
        }

        public bool FilterArgumentMatchesPattern(string value)
        {
            var matches = _filterRegex.Matches(value);
            return matches.Count > 0;
        }

        public bool ServerPortInValidRange(string value)
        {
            if (int.TryParse(value, out int portValue))
            {
                return (portValue >= _minServerPort && portValue <= _maxServerPort);
            }

            return false;
        }

        public override string ToString()
        {
            return $"Server: {ServerAddress} FilePath: {FilePath} Filter: {FileType}";
        }
    }
}
