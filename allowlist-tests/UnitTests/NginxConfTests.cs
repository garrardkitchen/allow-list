using System.IO;
using System.Threading.Tasks;
using AllowList.models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AllowListTests.UnitTests
{
    public class NginxConfTests
    {
        [Fact]
        public async Task Create_a_formatted_conf_file()
        {
            // arrange
            JObject raw = JObject.Parse(await File.ReadAllTextAsync($"data{Path.DirectorySeparatorChar}ServiceTags_Public_20200629.json"));
            var nginxConfString = new NginxConfString(raw.ToObject<Runners>());

            // act
            var content = await nginxConfString.Create();

            // assert
            Assert.NotEqual(0, content.Length);
        }
    }
}