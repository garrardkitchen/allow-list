using System.Net.Http;
using System.Threading.Tasks;

namespace whitelist.models
{
    public class AzureIPv4Ranges : IAzureIpv4s
    {
        private readonly HttpClient _httpClient;

        private readonly IGenerateFilename _generateFilename;

        // const string AZURE_URL = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519";
        private const string AZURE_URL =
            "https://download.microsoft.com/download/7/1/D/71D86715-5596-4529-9B13-DA13A5DE5B63/{0}.json";
        public AzureIPv4Ranges(HttpClient httpClient, IGenerateFilename generateFilename)
        {
            _httpClient = httpClient;
            _generateFilename = generateFilename;
        }
        
        public async Task<string> GetJsonFile()
        {
            var foo = await _httpClient.GetAsync(string.Format(AZURE_URL, _generateFilename.Create()));
            return await foo.Content.ReadAsStringAsync();
        }
    }
}