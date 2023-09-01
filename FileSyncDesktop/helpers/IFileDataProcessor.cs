namespace FileSyncDesktop.Helpers
{
    public interface IFileDataProcessor
    {
        Library.Helpers.BibRecords ProcessFile(string fileName);
    }
}