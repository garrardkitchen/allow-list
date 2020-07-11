using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Xml;
using whitelist.models;

namespace whiltelist
{
    class Program
    {
        private const string OutputGithubActionsRunnersConf = "./output/github-actions-runners.conf";
        private const string Output = "output";

        //https://www.microsoft.com/en-us/download/confirmation.aspx?id=56519

        static async Task Main(string[] args)
        {
            AzureIpv4s ipv4s = new AzureIpv4s(new HttpClient());
            AzureIpv4Parser parser = new AzureIpv4Parser(await ipv4s.GetJsonFile());
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
