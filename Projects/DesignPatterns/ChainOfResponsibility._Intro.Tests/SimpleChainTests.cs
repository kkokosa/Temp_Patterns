using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;

namespace ChainOfResponsibility._Intro.Tests
{
    public class SimpleChainTests
    {
        [Fact]
        public void Given_SampleChain_WhenUnknownHandler_ThenShouldThrowException()
        {
            var fsMock = new Mock<IFileSystem>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var source = new ResourceSource(fsMock.Object, handlerMock.Object);

            var exception = Record.Exception(() => source.Acquire(@"magic:\\fromOuterWorld"));

            Assert.NotNull(exception);
            Assert.IsType<NotImplementedException>(exception);
        }

        // Mocks approach
        [Fact]
        public void Given_SampleChain_WhenFileUrlRequested_ThenFileReadCalled()
        {
            var fileMock = new Mock<FileBase>();
            var fsMock = new Mock<IFileSystem>();
            fsMock.Setup(x => x.File).Returns(fileMock.Object);
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var source = new ResourceSource(fsMock.Object, handlerMock.Object);

            source.Acquire(@"file:\\C:\sample.txt");

            fileMock.Verify(x => x.ReadAllText(@"C:\sample.txt"));
        }

        // Stubs approach

        [Fact]
        public void Given_SampleChain_WhenFileUrlRequest_ThenFileContentReturned()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>()
            {
                [@"C:\sample.txt"] = "Hello world!"
            });
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            var source = new ResourceSource(fileSystem, handlerMock.Object);

            var result = source.Acquire(@"file:\\C:\sample.txt");

            Assert.Equal("Hello world!", result);
        }

        [Fact]
        public void Given_SampleChain_WhenHttpUrlRequest_ThenHttpContentReturned_WithMoq()
        {
            var fsMock = new Mock<IFileSystem>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Hello world!")
                })
                .Verifiable();

            var source = new ResourceSource(fsMock.Object, handlerMock.Object);

            var result = source.Acquire(@"https:\\get.me");

            Assert.NotNull(result);
            Assert.Equal("Hello world!", result);
            handlerMock.Protected()
                .Verify("SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get
                                                         && req.RequestUri == new Uri(@"https:\\get.me")),
                    ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public void Given_SampleChain_WhenHttpUrlRequest_ThenHttpContentReturned_WithMoq48()
        {
            var fsMock = new Mock<IFileSystem>();
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock.Protected().As<IHttpMessageHandler>()
                .Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("Hello world!")
                })
                .Verifiable();
            
            var source = new ResourceSource(fsMock.Object, handlerMock.Object);

            var result = source.Acquire(@"https:\\get.me");

            Assert.NotNull(result);
            Assert.Equal("Hello world!", result);
            handlerMock.Protected().As<IHttpMessageHandler>()
                .Verify(x => x.SendAsync(It.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get
                                                                          && req.RequestUri == new Uri(@"https:\\get.me")),
                    It.IsAny<CancellationToken>()), Times.Once());
        }

        interface IHttpMessageHandler
        {
            Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken token);
        }


        [Fact]
        public void Given_SampleChain_WhenHttpUrlRequest_ThenHttpContentReturned_WithMockHttp()
        {
            var fsMock = new Mock<IFileSystem>();
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect(HttpMethod.Get, @"https:\\get.me")
                .Respond("text/html", "Hello world!");
            
            var source = new ResourceSource(fsMock.Object, mockHttp);

            var result = source.Acquire(@"https:\\get.me");

            Assert.NotNull(result);
            Assert.Equal("Hello world!", result);
        }
    }
}