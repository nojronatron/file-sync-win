using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncDesktop.Library.Models
{
    public class BibRecordModel
    {
        public int BibNumber { get; set; }
        public string Action { get; set; }
        public string BibTimeOfDay { get; set; }
        public int DayOfMonth { get; set; }
        public string ShortLocation { get; set; }
    }
}
