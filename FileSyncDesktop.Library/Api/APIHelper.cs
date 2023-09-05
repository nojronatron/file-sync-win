using FileSyncDesktop.Library.Helpers;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FileSyncDesktop.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private static HttpClient _apiClient;
        public HttpClient ApiClient { get { return _apiClient; } }
        private readonly IFileWatcherSettings _fileWatcherSettings;

        public APIHelper(IFileWatcherSettings fileWatcherSettings)
        {
            _fileWatcherSettings = fileWatcherSettings;
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = $"https://{_fileWatcherSettings.ServerAddress}:{_fileWatcherSettings.ServerPort}/";
            _apiClient = new HttpClient
            {
                BaseAddress = new Uri(api)
            };

            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
