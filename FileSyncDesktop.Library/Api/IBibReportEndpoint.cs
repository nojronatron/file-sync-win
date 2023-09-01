using FileSyncDesktop.Library.Helpers;
using System;
using System.Threading.Tasks;

namespace FileSyncDesktop.Library.Api
{
    public interface IBibReportEndpoint
    {
        Task<Tuple<bool, string>> PostBibReport(BibRecords bibRecords);
    }
}