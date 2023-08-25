using System.Diagnostics.CodeAnalysis;

namespace FileSyncAPI.Models
{
    public class ApiBibRecord : IApiBibRecord
    {
        //public int? Id { get; set; } = null;
        public int? BibNumber { get; set; } = null;
        public string Action { get; set; } = string.Empty;
        public int? BibTimeOfDay { get; set; } = null;
        public int? DayOfMonth { get; set; } = null;
        public string Location { get; set; } = string.Empty;

        //public ApiBibRecord() { }

        //public ApiBibRecord(int id, int bibNumber, string action, string bibTimeOfDay, int dayOfMonth, string location)
        //{
        //    Id = id;
        //    BibNumber = bibNumber;
        //    Action = action;
        //    BibTimeOfDay = bibTimeOfDay;
        //    DayOfMonth = dayOfMonth;
        //    Location = location;
        //}

        public override string ToString()
        {
            return $"ApiBibRecord: {BibNumber}, {Action}, {BibTimeOfDay}, {DayOfMonth}, {Location}";
        }
    }
}
