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
    public class AzureUnhappyTests
    {
        private readonly Mock<IGenerateFilename> _mockGenerateFilename;
        private readonly Mock<HttpMessageHandler> handlerMock;

        // favouring plain english for unit testing names over GWT or Subject.Action.Outcome

        public AzureUnhappyTests()
        {
            handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _mockGenerateFilename = new Mock<IGenerateFilename>();
            _mockGenerateFilename.Setup(x => x.Create()).Returns("");
        }
        
        [Fact]
        public async Task Get_error_code_on_first_attempt_on_calling_endpoint()
        {
            // arrange
            handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadGateway,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                });
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };
            var azure = new AzureIPv4Ranges(httpClient, _mockGenerateFilename.Object);
            
            // act
            var jsonFile = await azure.GetJsonFile();

            // assert
            Assert.NotEmpty(jsonFile.Value);
            Assert.True(jsonFile.IsSuccess);
            _mockGenerateFilename.Verify(x=>x.Create(), Times.Exactly(2));
        }
        
        [Fact]
        public async Task Get_error_code_on_second_attempt_on_calling_endpoint()
        {
            // arrange
            handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadGateway,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadGateway,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                })
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                });
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };
            var azure = new AzureIPv4Ranges(httpClient, _mockGenerateFilename.Object);
            
            // act
            var jsonFile = await azure.GetJsonFile();

            // assert
            Assert.NotEmpty(jsonFile.Value);
            Assert.True(jsonFile.IsSuccess);
            _mockGenerateFilename.Verify(x=>x.Create(), Times.Exactly(3));
        }
        
        [Fact]
        public async Task Get_timeout_initially_from_calling_endpoint()
        {
            // arrange
            handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<TimeoutException>()
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'id':1,'value':'1'}]"),
                });
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };
            var azure = new AzureIPv4Ranges(httpClient, _mockGenerateFilename.Object);
            
            // act
            var jsonFile = await azure.GetJsonFile();

            // assert
            Assert.NotEmpty(jsonFile.Value);
            Assert.True(jsonFile.IsSuccess);
            _mockGenerateFilename.Verify(x=>x.Create(), Times.Exactly(2));
        }
        
        [Fact]
        public async Task Get_timeout_initially_then_nothing_from_calling_endpoint_expecting_a_complete_failure()
        {
            // arrange
            handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Throws<TimeoutException>()
                .Throws<TimeoutException>()
                .Throws<TimeoutException>();
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/"),
            };
            var azure = new AzureIPv4Ranges(httpClient, _mockGenerateFilename.Object);
            
            // act
            var jsonFile = await azure.GetJsonFile();

            // assert
            Assert.True(jsonFile.IsFailure);
            Assert.Equal("Failed to get a response", AzureIPv4Ranges.FailureToGetAResponseMessage);
            _mockGenerateFilename.Verify(x=>x.Create(), Times.Exactly(3));
        }
    }
}
