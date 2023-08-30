using FileSyncDesktop.Library.Models;
using FileSyncDesktop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncDesktop.Collections
{
    public class BibRecordCollection : ICollection<BibRecordModel>, IBibRecordCollection
    {
        // wrapper class represents a List of BibRecord objects

        IList<BibRecordModel> _BibRecords = new List<BibRecordModel>();
        public BibRecordCollection() { }

        public int Count => _BibRecords.Count;

        public bool IsReadOnly => false;

        public void Add(BibRecordModel item)
        {
            _BibRecords.Add(item);
        }

        /// <summary>
        /// Accepts an ICollection of BibRecord instances and adds it to this collection.
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IList<BibRecordModel> items)
        {
            if (items != null && items.Count > 0)
            {
                foreach (var item in items)
                {
                    _BibRecords.Add(item);
                }
            }
        }

        public void Clear()
        {
            _BibRecords.Clear();
        }

        public bool Contains(BibRecordModel item)
        {
            return _BibRecords.Contains(item);
        }

        public void CopyTo(BibRecordModel[] array, int arrayIndex)
        {
            foreach (var BibRecord in _BibRecords)
            {
                array[arrayIndex] = BibRecord;
                arrayIndex++;
            }
        }

        public IEnumerator<BibRecordModel> GetEnumerator()
        {
            return _BibRecords.GetEnumerator();
        }

        public bool Remove(BibRecordModel item)
        {
            return _BibRecords.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _BibRecords.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Collection size:").Append(_BibRecords.Count).Append('\n');

            foreach (var ApiBibRecord in _BibRecords)
            {
                sb.Append(ApiBibRecord.ToString()).Append('\n');
            }

            return sb.ToString();
        }
    }
}
