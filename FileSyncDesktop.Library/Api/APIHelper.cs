using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FileSyncDesktop.Library.Api
{
    public class APIHelper : IAPIHelper
    {
        private static HttpClient _apiClient;
        public HttpClient ApiClient { get { return _apiClient; } }

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            string api = ConfigurationManager.AppSettings["api"];

            if (api == null || api == string.Empty)
            {
                api = "api";
            }

            _apiClient = new HttpClient();
            _apiClient.BaseAddress = new Uri(api);
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
