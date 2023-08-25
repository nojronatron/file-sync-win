namespace FileSyncAPI.Models
{
    public interface IApiBibDataCollection
    {
        int Count { get; }
        bool IsReadOnly { get; }

        void Add(ApiBibRecord item);
        void Clear();
        bool Contains(ApiBibRecord item);
        void CopyTo(ApiBibRecord[] array, int arrayIndex);
        IEnumerator<ApiBibRecord> GetEnumerator();
        bool Remove(ApiBibRecord item);
        string ToString();
    }
}