using FileSyncAPI.Models;

namespace FileSyncAPI.Helpers
{
    public class ConvertBibRecord
    {
        // accepts an ApiBibRecord and returns a tab delimited string
        public static string Convert(ApiBibRecord record)
        {
            return $"{record.BibNumber}\t{record.Action}\t{record.BibTimeOfDay}\t{record.DayOfMonth}\t{record.Location}";
        }

        // accepts a tab delimited string and returns an ApiBibRecord
        public static ApiBibRecord Convert(string record)
        {
            string[] fields = record.Split('\t');

            ApiBibRecord apiBibRecord = new()
            {
                BibNumber = int.Parse(fields[0]),
                Action = fields[1],
                BibTimeOfDay = int.Parse(fields[2]),
                DayOfMonth = int.Parse(fields[3]),
                Location = fields[4]
            };
            
            return apiBibRecord;
        }
    }
}
