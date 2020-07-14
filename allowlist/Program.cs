using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AllowList.models;

namespace AllowList
{
    class Program
    {
        private const string OutputGithubActionsRunnersConf = "./output/github-actions-runners.conf";
        private const string Output = "output";

        //https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519

        static async Task Main(string[] args)
        {
            AzureIPv4Ranges iPv4Ranges = new AzureIPv4Ranges(new HttpClient(), new GenerateFilename(DateTime.Now));
            var ranges = await iPv4Ranges.GetJsonFile();
            if (ranges.IsSuccess)
            {
                AzureIPv4Parser parser = new AzureIPv4Parser(ranges.Value);
                NginxConfString confString = new NginxConfString(await parser.Parse());
            
                var directoryInfo = Directory.CreateDirectory(Output);
                using (var output = new StreamWriter(OutputGithubActionsRunnersConf, false, Encoding.UTF8))
                {
                    await output.WriteAsync(await confString.Create());
                    await output.FlushAsync();
                }
            }
        }
    }
}
