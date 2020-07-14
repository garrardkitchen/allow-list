using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllowList.models
{
    public class NginxConfString
    {
        private readonly Runners _json;
        static List<string> _regions = new List<string>{"AzureCloud.eastus","AzureCloud.eastus2", "AzureCloud.westus2", "AzureCloud.centralus", "AzureCloud.southcentralus"};

        public NginxConfString(Runners json)
        {
            _json = json;
        }

        public Task<StringBuilder> Create()
        {
            var list = from j in _json.Values 
                where _regions.Contains(j.Name)
                select j.Properties;
            StringBuilder output = new StringBuilder();
            foreach (var item in list)
            {
                Console.WriteLine(item.Region);
                output.AppendLine($"    # GitHub Actions Runner in the {item.Region} region  {Environment.NewLine}");

                item.AddressPrefixes.ForEach(x =>
                {
                    string line = $"    allow {x}; # allow Azure Outbound IPv4 Address";
                    output.AppendLine(line);
                    Console.WriteLine(line);
                });
                output.AppendLine("");
            }

            return Task.FromResult(output);
        }
    }
}