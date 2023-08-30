using FileSyncDesktop.Library.Helpers;
using System.Threading.Tasks;

namespace FileSyncDesktop.Library.Api
{
    public interface IBibReportEndpoint
    {
        Task PostBibReport(BibRecords bibRecords);
    }
}