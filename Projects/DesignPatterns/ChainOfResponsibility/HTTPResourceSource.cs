using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ChainOfResponsibility
{
    public class HttpResourceSource : ResourceSource
    {
        private readonly HttpMessageHandler httpMessageHandler;

        public HttpResourceSource()
        {
            this.httpMessageHandler = new HttpClientHandler();
        }

        public HttpResourceSource(HttpMessageHandler httpMessageHandler)
        {
            this.httpMessageHandler = httpMessageHandler;
        }

        private const string pattern = @"https?:\\\\(.*)";


        protected override bool CanHandle(string handle)
        {
            return Regex.IsMatch(handle, pattern);
        }

        protected override string InternalAcquire(string handle)
        {
            HttpClient client = new HttpClient(httpMessageHandler);
            return client.GetStringAsync(handle).GetAwaiter().GetResult();
        }
    }
}