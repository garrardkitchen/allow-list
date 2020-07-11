using System.Threading.Tasks;
using Newtonsoft.Json;

namespace whitelist.models
{
    public class AzureIpv4Parser
    {
        private readonly string _raw;

        public AzureIpv4Parser(string raw)
        {
            _raw = raw;
        }
        
        public async Task<Runners> Parse()
        {
            return JsonConvert.DeserializeObject<Runners>(_raw);
        }
    }
}