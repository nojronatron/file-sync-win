using FileSyncDesktop.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
