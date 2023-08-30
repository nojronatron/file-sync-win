using FileSyncAPI.Models;

namespace FileSyncAPI.Helpers
{
    public interface IRecordLogger
    {
        bool IsEnabled { get; set; }

        void AddEntry(ApiBibRecord record);
        void Dispose();
        void FlushEntries();
    }
}