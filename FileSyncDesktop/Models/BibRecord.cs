using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncDesktop.Models
{
    public class BibRecord
    {
        public int Id { get; set; }
        public int BibNumber { get; set; }
        public string Action { get; set; }
        public string BibTimeOfDay { get; set; }
        public int DayOfMonth { get; set; }
        public string Location { get; set; }
        public string ShortLocation { get; set; }

        public BibRecord() { }
        public BibRecord(int id, int bibNumber, string action, string bibTimeOfDay, int dayOfMonth, string location, string shortLocation)
        {
            Id = id;
            BibNumber = bibNumber;
            Action = action;
            BibTimeOfDay = bibTimeOfDay;
            DayOfMonth = dayOfMonth;
            Location = location;
            ShortLocation = shortLocation;
        }
    }
}
