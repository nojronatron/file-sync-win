namespace FileSyncDesktop.Models
{
    public interface IFileWatcherSettings
    {
        string FilePath { get; set; }
        string FileType { get; set; }
        string ServerAddress { get; set; }
        string ServerPort { get; set; }

        void GetSettingsFromEnvVars();
        bool HasFileSettings();
        bool HasServerSettings();
        void SetFileSettings(string filePath, string fileType);
        void SetServerSettings(string serverAddress, int serverPort);
        void RemoveFileSettings();
        void RemoveServerSettings();
        string ToString();
    }
}