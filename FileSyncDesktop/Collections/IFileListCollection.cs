using System.Collections.Generic;

namespace FileSyncDesktop.Collections
{
    public interface IFileListCollection
    {
        int Count { get; }
        List<string> FileList { get; set; }
        bool IsReadOnly { get; }

        void Add(string item);
        void Clear();
        bool Contains(string item);
        void CopyTo(string[] array, int arrayIndex);
        IEnumerator<string> GetEnumerator();
        bool Remove(string item);
        string ToString();
    }
}