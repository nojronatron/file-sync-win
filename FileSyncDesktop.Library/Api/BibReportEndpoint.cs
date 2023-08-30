using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FileSyncDesktop.Library.Helpers;
using FileSyncDesktop.Library.Models;

namespace FileSyncDesktop.Library.Api
{
    public class BibReportEndpoint : IBibReportEndpoint
    {
        private IAPIHelper _apiHelper;

        public BibReportEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostBibReport(BibRecords bibRecords)
        {
            var requestUri = "/api/BibRecords";
            var httpContent = new StringContent(bibRecords.ToJson(), Encoding.UTF8, "application/json");

            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(requestUri, httpContent))
            {
                if (response.IsSuccessStatusCode)
                {
                    // todo: log success
                    Console.WriteLine("Success");
                }
                else
                {
                    // todo: log failure
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }
    }
}
