using FileSyncDesktop.Collections;
using FileSyncDesktop.Models;
using System.Collections;
using System.Collections.Generic;

namespace FileSyncDesktop.Helpers
{
    public interface IFileDataProcessor
    {
        Library.Helpers.BibRecords ProcessFile(string fileName);
    }
}