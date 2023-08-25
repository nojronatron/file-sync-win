namespace FileSyncAPI.Models
{
    public interface IApiBibRecord
    {
        string Action { get; set; }
        int? BibNumber { get; set; }
        int? BibTimeOfDay { get; set; }
        int? DayOfMonth { get; set; }
        string Location { get; set; }

        string ToString();
    }
}