namespace FileSyncDesktop.Helpers
{
    public interface ILogger
    {
        bool IsEnabled { get; set; }

        void Data(string name, string info);
        void Dispose();
        void Flush();
    }
}