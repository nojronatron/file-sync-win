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
            string message = string.Empty;

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
                message = $"Argument Null Exception was thrown.\nStacktrace: {ArgNullEx.StackTrace}\nMessage: {ArgNullEx.Message}";
                if (ArgNullEx.InnerException != null)
                {
                    message += $"\nInner Exception Message: {ArgNullEx.InnerException.Message}";
                }
            }
            catch (HttpRequestException HttpReqEx)
            {
                message = $"HTTP Request Exception was thrown.\nStacktrace: {HttpReqEx.StackTrace}\nMessage: {HttpReqEx.Message}";
                if (HttpReqEx.InnerException != null)
                {
                    message += $"\nInner Exception Message: {HttpReqEx.InnerException.Message}";
                }
            }
            catch (Exception ex)
            {
                message = $"Some other Exception was thrown.\nStacktrace: {ex.StackTrace}\nMessage: {ex.Message}";
            }

            return new Tuple<bool, string>(result, message);
        }
    }
}
