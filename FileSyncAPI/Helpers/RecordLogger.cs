using FileSyncAPI.Models;

namespace FileSyncAPI.Helpers
{
    /// <summary>
    /// Class will log records received by the API to a file.
    /// </summary>
    public class RecordLogger : IDisposable, IRecordLogger
    {
        private readonly static string LogFileName = "Received-Bib-Reports.txt";
        private FileInfo LogfileInfo { get; set; }
        private readonly Queue<string>? _records = null;
        public bool IsEnabled { get; set; } = false;

        public RecordLogger()
        {
            DirectoryInfo rootDirectory = new(Directory.GetCurrentDirectory());
            LogfileInfo = new FileInfo(Path.Combine(rootDirectory.FullName, LogFileName));
            _records = new Queue<string>();
            IsEnabled = true;
        }

        public void AddEntry(ApiBibRecord record)
        {
            // transform a ApiBibRecord into a tab delimited string
            string recordString = ConvertBibRecord.Convert(record);

            if (_records != null && IsEnabled)
            {
                _records.Enqueue(recordString);
            }
        }

        public void FlushEntries()
        {
            if (IsEnabled)
            {
                using StreamWriter sw = LogfileInfo.AppendText();
                while (_records != null && _records.Count > 0)
                {
                    sw.WriteLine(_records.Dequeue());
                }
            }
        }

        public void Dispose()
        {
            FlushEntries();
            _records?.Clear();
            IsEnabled = false;
            GC.SuppressFinalize(this); // prevents derived classes from implementing a finalizer
        }
    }
}
