using FileSyncDesktop.Models;
using System.Collections.Generic;

namespace FileSyncDesktop.Collections
{
    public interface IBibRecordCollection
    {
        int Count { get; }
        bool IsReadOnly { get; }

        void Add(BibRecord item);
        void Clear();
        bool Contains(BibRecord item);
        void CopyTo(BibRecord[] array, int arrayIndex);
        IEnumerator<BibRecord> GetEnumerator();
        bool Remove(BibRecord item);
    }
}