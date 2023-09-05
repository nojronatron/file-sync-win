namespace FileSyncDesktop.Library.Helpers
{
    public interface IFileWatcherSettings
    {
        string FilePath { get; set; }
        string FileType { get; set; }
        string ServerAddress { get; set; }
        string ServerPort { get; set; }

        bool FileSourcePathIsValid(string value);
        bool FilterArgumentIsValid(string value);
        void GetSettingsFromEnvVars();
        bool HasFileSettings();
        bool HasServerSettings();
        void RemoveFileSettings();
        void RemoveServerSettings();
        bool ServerPortIsValid(string value);
        void SetFileSettings(string filePath, string fileType);
        void SetServerSettings(string serverAddress, string serverPort);
        string ToString();
    }
}