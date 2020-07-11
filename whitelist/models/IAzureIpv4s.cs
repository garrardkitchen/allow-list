using System.Threading.Tasks;

namespace whitelist.models
{
    public interface IAzureIpv4s
    {
        Task<string> GetJsonFile();
    }
}