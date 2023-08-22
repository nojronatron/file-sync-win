using System;
using System.Collections.Generic;
using System.IO;

namespace file_sync_win.FileSyncDesktop.Helpers
{
    internal class Logger : IDisposable
    {
        private readonly static string LogFileName = "file-sync-win.log";
        private FileInfo LogfileInfo { get; set; }
        private Queue<string> LogEntries = null;
        public bool IsEnabled { get; set; } = false;

        public Logger()
        {
            DirectoryInfo rootDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            LogfileInfo = new FileInfo(Path.Combine(rootDirectory.FullName, LogFileName));
            LogEntries = new Queue<string>();
            IsEnabled = true;
        }

        public void Data(string name, string info)
        {
            LogEntries.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{name}] {info}");
        }

        public void Flush()
        {
            // use stream writer to write each entry in the LogEntries queue until it's empty
            if (IsEnabled)
            {
                using (StreamWriter sw = LogfileInfo.AppendText())
                {
                    while (LogEntries.Count > 0)
                    {
                        sw.WriteLine(LogEntries.Dequeue());
                    }
                }
            }
        }

        public void Dispose()
        {
            this.Flush();
            LogEntries = null;
            IsEnabled = false;
        }
    }
}
