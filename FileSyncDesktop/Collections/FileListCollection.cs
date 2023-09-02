using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FileSyncDesktop.Collections
{
    public class FileListCollection : ICollection<string>, IFileListCollection
    {
        private List<string> _fileList;
        public List<string> FileList
        {
            get { return _fileList; }
            set { _fileList = value; }
        }

        public int Count => _fileList.Count;
        public bool IsReadOnly => false;

        public FileListCollection()
        {
            _fileList = new List<string>();
        }

        public void Add(string item)
        {
            _fileList.Add(item);
        }

        public void Clear()
        {
            _fileList.Clear();
        }

        public bool Contains(string item)
        {
            return _fileList.Contains(item);
        }

        /// <summary>
        /// Copy elements of this collection to an array starting at arrayIndex.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void CopyTo(string[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || arrayIndex >= Count)
            {
                foreach (string item in _fileList)
                {
                    array[arrayIndex] = item;
                    arrayIndex++;
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _fileList.GetEnumerator();
        }

        public bool Remove(string item)
        {
            return _fileList.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            if (_fileList.Count == 0)
            {
                return "no files";
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                foreach (string item in _fileList)
                {
                    sb.Append(item).Append("\n");
                }
                return sb.ToString();
            }
        }
    }
}
