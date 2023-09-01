using FileSyncDesktop.Library.Models;
using System.Collections.Generic;

namespace FileSyncDesktop.Library.Helpers
{
    public class BibRecordModels
    {
        public List<BibRecordModel> BibRecords { get; set; } = new List<BibRecordModel>();

        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }
    }
}
