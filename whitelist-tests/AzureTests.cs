using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using whitelist.models;
using Xunit;

namespace whitelist_tests
{
    public class UnitTest1
    {
        private HttpClient _httpClient;
        
        public UnitTest1()
        {

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .Verifiable();
            _httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };

        }
        [Fact]
        public async Task Returns_Something()
        {
            // arrange
            var azure = new AzureIpv4s(_httpClient);

            // act
            var jsonFile = await azure.GetJsonFile();

            // assert
            Assert.NotEmpty(jsonFile);
        }
        
        [Fact]
        public async Task Parse_String_Into_Json()
        {
            // arrange
            JObject raw = JObject.Parse(await File.ReadAllTextAsync($"data{Path.DirectorySeparatorChar}ServiceTags_Public_20200629.json")); 
            var azure = new AzureIpv4Parser(raw.ToString());

            // act
            Runners json = await azure.Parse();

            // assert
            Assert.Equal(96, json.ChangeNumber);
            Assert.Equal("Public", json.Cloud);
        }
        
        [Fact]
        public async Task Create_Formatted_File()
        {
            // arrange
            JObject raw = JObject.Parse(await File.ReadAllTextAsync($"data{Path.DirectorySeparatorChar}ServiceTags_Public_20200629.json"));
            var nginxConfString = new NginxConfString(raw.ToObject<Runners>());

            // act
            var content = await nginxConfString.Create();

            // assert
            Assert.NotEqual(0, content.Length);
        }

        public class Item
        {
            public DateTime Date { get; set; }
            public string Expected { get; set; }
        }

        public static IEnumerable<object[]> GetData()
        {
            yield return new object[]
            {
                new Item {Date = new DateTime(2020, 06, 01), Expected = "ServiceTags_Public_20200601"},
            };
            
            yield return new object[]
            {
                new Item {Date = new DateTime(2021, 10, 11), Expected = "ServiceTags_Public_20211011"},
            };
            
            yield return new object[]
            {
                new Item {Date = new DateTime(2030, 01, 01), Expected = "ServiceTags_Public_20300101"}
            };
        }
        
        [Theory]
        [MemberData(nameof(GetData))]
        public void Create_File_Name(Item item)
        {
            // arrange
            var generateFileName = new GenerateFileName(item.Date);
            
            // act
            var result = generateFileName.Create();

            // assert
            Assert.Equal(item.Expected, result);
        }
    }
}
