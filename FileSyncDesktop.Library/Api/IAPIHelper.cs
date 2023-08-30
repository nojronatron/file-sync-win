using System.Net.Http;

namespace FileSyncDesktop.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
    }
}