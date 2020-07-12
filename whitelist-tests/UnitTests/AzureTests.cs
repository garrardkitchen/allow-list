using System;
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

namespace whitelist_tests.UnitTests
{
    public class AzureTests
    {
        private HttpClient _httpClient;
        private Mock<IGenerateFilename> _mockGenerateFilename;

        public AzureTests()
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

            _mockGenerateFilename = new Mock<IGenerateFilename>();
            _mockGenerateFilename.Setup(x => x.Create()).Returns("");
        }
        
        [Fact]
        public async Task Get_something_back_from_request()
        {
            // arrange
            var azure = new AzureIPv4Ranges(_httpClient, _mockGenerateFilename.Object);

            // act
            var jsonFile = await azure.GetJsonFile();

            // assert
            Assert.NotEmpty(jsonFile);
        }
        
        [Fact]
        public async Task Parse_response_into_valid_json()
        {
            // arrange
            JObject raw = JObject.Parse(await File.ReadAllTextAsync($"data{Path.DirectorySeparatorChar}ServiceTags_Public_20200629.json")); 
            var azure = new AzureIPv4Parser(raw.ToString());

            // act
            Runners json = await azure.Parse();

            // assert
            Assert.Equal(96, json.ChangeNumber);
            Assert.Equal("Public", json.Cloud);
        }
    }
}
