using System.Collections;
using System.Text;

namespace FileSyncAPI.Models
{
    public class ApiBibDataCollection : ICollection<ApiBibRecord>, IApiBibDataCollection
    {
        // wrapper class represents a List of ApiBibRecord objects

        readonly IList<ApiBibRecord> _apiBibRecords = new List<ApiBibRecord>();
        public ApiBibDataCollection() { }
        public ApiBibDataCollection(IList<ApiBibRecord> ApiBibRecords)
        {
            _apiBibRecords = ApiBibRecords;
        }

        public int Count => _apiBibRecords.Count;

        public bool IsReadOnly => false;

        public void Add(ApiBibRecord item)
        {
            _apiBibRecords.Add(item);
        }

        public void Clear()
        {
            _apiBibRecords.Clear();
        }

        public bool Contains(ApiBibRecord item)
        {
            return _apiBibRecords.Contains(item);
        }

        public void CopyTo(ApiBibRecord[] array, int arrayIndex)
        {
            foreach (var ApiBibRecord in _apiBibRecords)
            {
                array[arrayIndex] = ApiBibRecord;
                arrayIndex++;
            }
        }

        public IEnumerator<ApiBibRecord> GetEnumerator()
        {
            return _apiBibRecords.GetEnumerator();
        }

        public bool Remove(ApiBibRecord item)
        {
            return _apiBibRecords.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _apiBibRecords.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append("Collection size:").Append(_apiBibRecords.Count).Append('\n');

            foreach (var ApiBibRecord in _apiBibRecords)
            {
                sb.Append(ApiBibRecord.ToString()).Append('\n');
            }

            return sb.ToString();
        }
    }
}
