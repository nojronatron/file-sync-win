using System;

namespace FileSyncDesktop.Helpers
{
    public interface IRmzLogger : IDisposable
    {
        bool IsEnabled { get; set; }

        void Data(string name, string info);
        void Flush();
    }
}