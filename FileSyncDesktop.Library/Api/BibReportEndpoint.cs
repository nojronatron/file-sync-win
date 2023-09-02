using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FileSyncDesktop.Library.Helpers;

namespace FileSyncDesktop.Library.Api
{
    public class BibReportEndpoint : IBibReportEndpoint
    {
        private readonly IAPIHelper _apiHelper;

        public BibReportEndpoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<Tuple<bool, string>> PostBibReport(BibRecordModels bibRecords)
        {
            var requestUri = "/api/BibRecords";
            var httpContent = new StringContent(bibRecords.ToJson(), Encoding.UTF8, "application/json");
            bool result = false;
            string message = "";

            try
            {
                using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsync(requestUri, httpContent))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = true;
                        message = "Succeeded.";
                    }
                }
            }
            catch (ArgumentNullException ArgNullEx)
            {
                message = ArgNullEx.Message;
            }
            catch (HttpRequestException HttpReqEx)
            {
                message = HttpReqEx.Message;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return new Tuple<bool, string>(result, message);
        }
    }
}
