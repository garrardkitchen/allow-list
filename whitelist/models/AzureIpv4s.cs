using System.Net.Http;
using System.Threading.Tasks;

namespace whitelist.models
{
    public class AzureIpv4s : IAzureIpv4s
    {
        private readonly HttpClient _httpClient;
        // const string AZURE_URL = "https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519";
        private const string AZURE_URL =
            "https://download.microsoft.com/download/7/1/D/71D86715-5596-4529-9B13-DA13A5DE5B63/ServiceTags_Public_20200706.json";
        public AzureIpv4s(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<string> GetJsonFile()
        {
            var foo = await _httpClient.GetAsync(AZURE_URL);
            return await foo.Content.ReadAsStringAsync();
        }
    }
}