using System;
using System.Text.RegularExpressions;

namespace FileSyncDesktop.Library.Helpers
{
    public class FileWatcherSettings : IFileWatcherSettings
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileType { get; set; } = string.Empty;
        public string ServerAddress { get; set; } = string.Empty;
        public string ServerPort { get; set; } = string.Empty;
        private static readonly Regex _filepathRegex = new Regex(@"^\w:\\((\S)*\\{0,1})*$");
        private static readonly Regex _filterRegex = new Regex(@"^\*\.\w{0,4}$");
        private static readonly int _minServerPort = 8001;
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
            return string.IsNullOrWhiteSpace(FilePath) && FileType.Substring(0, 2) == "*.";
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
            return ServerAddress != string.Empty && ServerPort != string.Empty;
        }

        public bool FileSourcePathIsValid(string value)
        {
            var matches = _filepathRegex.Matches(value);

            if (matches.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool FilterArgumentIsValid(string value)
        {
            var matches = _filterRegex.Matches(value);

            if (matches.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool ServerPortIsValid(string value)
        {
            if (int.TryParse(value, out int portValue))
            {

                if (portValue >= _minServerPort && portValue <= _maxServerPort)
                {
                    return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return $"Server: {ServerAddress} FilePath: {FilePath} Filter: {FileType}";
        }
    }
}
