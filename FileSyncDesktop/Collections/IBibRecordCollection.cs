using FileSyncDesktop.Library.Helpers;
using FileSyncDesktop.Library.Models;
using FileSyncDesktop.Models;
using System.Collections.Generic;

namespace FileSyncDesktop.Collections
{
    public interface IBibRecordCollection
    {
        int Count { get; }
        bool IsReadOnly { get; }

        void Add(BibRecordModel item);
        void AddRange(IList<BibRecordModel> items);
        void Clear();
        bool Contains(BibRecordModel item);
        void CopyTo(BibRecordModel[] array, int arrayIndex);
        IEnumerator<BibRecordModel> GetEnumerator();
        bool Remove(BibRecordModel item);
    }
}