namespace FileSyncDesktop.Library.Models
{
    public class BibRecordModel
    {
        public int BibNumber { get; set; }
        public string Action { get; set; }
        public string BibTimeOfDay { get; set; }
        public int DayOfMonth { get; set; }
        public string Location { get; set; }

        public override string ToString()
        {
            return $"{BibNumber}\t{Action}\t{BibTimeOfDay}\t{DayOfMonth}\t{Location}";
        }
    }
}
