using System;
using System.Collections.Generic;
using System.IO;

namespace file_sync_win.helpers
{
    internal class Logger
    {
        private readonly static string LogFileName = "file-sync-win.log";
        private FileInfo LogfileInfo { get; set; }
        private Queue<string> LogEntries = null;
        public bool isEnabled { get; set; } = false;

        public Logger()
        {
            DirectoryInfo rootDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
            LogfileInfo = new FileInfo(Path.Combine(rootDirectory.FullName, LogFileName));
            LogEntries = new Queue<string>();
            isEnabled = true;
        }

        public void Data(string name, string info)
        {
            LogEntries.Enqueue($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")} [{name}] {info}");
        }

        public void Flush()
        {
            using (StreamWriter streamwriter = File.AppendText(LogfileInfo.FullName))
            {
                while (LogEntries.Count > 0)
                {
                    streamwriter.WriteLine(LogEntries.Dequeue());
                }
            }
        }
    }
}
