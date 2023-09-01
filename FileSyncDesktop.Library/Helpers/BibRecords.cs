using FileSyncDesktop.Library.Models;
using System.Collections.Generic;

namespace FileSyncDesktop.Library.Helpers
{
    public class BibRecords
    {
        public List<BibRecordModel> bibRecords { get; set; } = new List<BibRecordModel>();

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
