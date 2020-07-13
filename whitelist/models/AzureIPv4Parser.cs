using System.Threading.Tasks;
using Newtonsoft.Json;

namespace whitelist.models
{
    public class AzureIPv4Parser
    {
        private readonly string _raw;

        public AzureIPv4Parser(string raw)
        {
            _raw = raw;
        }
        
        public Task<Runners> Parse()
        {
            return Task.FromResult(JsonConvert.DeserializeObject<Runners>(_raw));
        }
    }
}