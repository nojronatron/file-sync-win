using FileSyncDesktop.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncDesktop.Collections
{
    public class BibRecordCollection : ICollection<BibRecord>, IBibRecordCollection
    {
        // wrapper class represents a List of BibRecord objects

        IList<BibRecord> _BibRecords = new List<BibRecord>();
        public BibRecordCollection() { }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(BibRecord item)
        {
            _BibRecords.Add(item);
        }

        public void Clear()
        {
            _BibRecords.Clear();
        }

        public bool Contains(BibRecord item)
        {
            return _BibRecords.Contains(item);
        }

        public void CopyTo(BibRecord[] array, int arrayIndex)
        {
            foreach (var BibRecord in _BibRecords)
            {
                array[arrayIndex] = BibRecord;
                arrayIndex++;
            }
        }

        public IEnumerator<BibRecord> GetEnumerator()
        {
            return _BibRecords.GetEnumerator();
        }

        public bool Remove(BibRecord item)
        {
            return _BibRecords.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _BibRecords.GetEnumerator();
        }
    }
}
