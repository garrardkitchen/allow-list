using System;
using System.Net.Http;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Polly;

namespace AllowList.models
{
    public class AzureIPv4Ranges : IAzureIpv4Ranges
    {
        private readonly HttpClient _httpClient;

        private readonly IGenerateFilename _generateFilename;

        public static string FailureToGetAResponseMessage = "Failed to get a response";

        // const string AZURE_URL = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519";
        private const string AZURE_URL =
            "https://download.microsoft.com/download/7/1/D/71D86715-5596-4529-9B13-DA13A5DE5B63/{0}.json";
        public AzureIPv4Ranges(HttpClient httpClient, IGenerateFilename generateFilename)
        {
            _httpClient = httpClient;
            _generateFilename = generateFilename;
        }
        
        public async Task<Result<string>> GetJsonFile()
        {
            try
            {
                var response = await Policy
                    .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                    .Or<TimeoutException>()
                    .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, span, retryCount, context) =>
                    {
                        if (result.Exception is TimeoutException)
                        {
                            Console.WriteLine(
                                $"Request failed with {result.Exception.Message}. Waiting {span} before retrying. Retry attempt {retryCount}");
                        }
                        else
                        {
                            Console.WriteLine(
                                $"Request failed with {result.Result.StatusCode}. Waiting {span} before retrying. Retry attempt {retryCount}");
                        }

                        if (retryCount == 3)
                        {
                            throw new Exception(FailureToGetAResponseMessage);
                        }

                    })
                    .ExecuteAsync(() => _httpClient.GetAsync(string.Format(AZURE_URL, _generateFilename.Create())));

                var result = await response.Content.ReadAsStringAsync();
                return Result.Success(result);
            }
            catch (Exception ex)
            {
                return Result.Failure<string>(ex.Message);
            }
        }
    }
}