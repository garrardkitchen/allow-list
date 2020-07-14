using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace AllowList.models
{
    public interface IAzureIpv4Ranges
    {
        Task<Result<string>> GetJsonFile();
    }
}