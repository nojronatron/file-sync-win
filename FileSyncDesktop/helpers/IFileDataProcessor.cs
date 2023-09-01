namespace FileSyncDesktop.Helpers
{
    public interface IFileDataProcessor
    {
        Library.Helpers.BibRecordModels ProcessFile(string fileName);
    }
}