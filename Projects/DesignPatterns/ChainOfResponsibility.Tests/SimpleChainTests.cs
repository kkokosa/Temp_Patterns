using Moq;
using Moq.Protected;
using RichardSzalay.MockHttp;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ChainOfResponsibility.Tests
{
    public class SimpleChainTests
    {
        [Fact]
        public void Given_SampleChain_WhenUnknownHandler_ThenShouldThrowException()
        {
            var fsMock = new Mock<IFileSystem>();
            var chain = new HttpResourceSource();
            var file = new FileResourceSource(fsMock.Object);

            var ftp = new FtpResourceSource();
            chain.SetNext(file);
            file.SetNext(ftp);

            var exception = Record.Exception(() => chain.Acquire(@"magic:\\fromOuterWorld"));

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

            var chain = new HttpResourceSource();
            var file = new FileResourceSource(fsMock.Object);
            var ftp = new FtpResourceSource();
            chain.SetNext(file);
            file.SetNext(ftp);

            chain.Acquire(@"file:\\C:\sample.txt");

            fileMock.Verify(x => x.ReadAllText(@"C:\sample.txt"));
        }

        // Stubs approach

        [Fact]
        public void Given_SampleChain_WhenFileUrlRequest_ThenFileContentReturned()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>() {
                [ @"C:\sample.txt"] = "Hello world!"
            });

            var chain = new HttpResourceSource();
            var file = new FileResourceSource(fileSystem);
            var ftp = new FtpResourceSource();
            chain.SetNext(file);
            file.SetNext(ftp);

            var result = chain.Acquire(@"file:\\C:\sample.txt");

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

            var chain = new HttpResourceSource(handlerMock.Object);
            var file = new FileResourceSource(fsMock.Object);
            var ftp = new FtpResourceSource();
            chain.SetNext(file);
            file.SetNext(ftp);

            var result = chain.Acquire(@"https:\\get.me");

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

            var chain = new HttpResourceSource(handlerMock.Object);
            var file = new FileResourceSource(fsMock.Object);
            var ftp = new FtpResourceSource();
            chain.SetNext(file);
            file.SetNext(ftp);

            var result = chain.Acquire(@"https:\\get.me");

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

            var chain = new HttpResourceSource(mockHttp);
            var file = new FileResourceSource(fsMock.Object);
            
            var ftp = new FtpResourceSource();
            chain.SetNext(file);
            file.SetNext(ftp);

            var result = chain.Acquire(@"https:\\get.me");

            Assert.NotNull(result);
            Assert.Equal("Hello world!", result);
        }
    }
}
