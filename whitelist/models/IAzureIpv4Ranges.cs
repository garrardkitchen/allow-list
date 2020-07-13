using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace whitelist.models
{
    public interface IAzureIpv4Ranges
    {
        Task<Result<string>> GetJsonFile();
    }
}