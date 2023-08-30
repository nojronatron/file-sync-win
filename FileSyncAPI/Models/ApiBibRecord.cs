using System.Diagnostics.CodeAnalysis;

namespace FileSyncAPI.Models
{
    public class ApiBibRecord : IApiBibRecord
    {
        public int? BibNumber { get; set; } = null;
        public string Action { get; set; } = string.Empty;
        public int? BibTimeOfDay { get; set; } = null;
        public int? DayOfMonth { get; set; } = null;
        public string Location { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"ApiBibRecord: {BibNumber}, {Action}, {BibTimeOfDay}, {DayOfMonth}, {Location}";
        }
    }
}
