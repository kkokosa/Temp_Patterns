using System;
using System.IO.Abstractions;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace ChainOfResponsibility._Intro
{
    public class ResourceSource : IResourceSource
    {
        private const string filePattern = @"file:\\\\(.*)";
        private const string httpPattern = @"https?:\\\\(.*)";
        private const string ftpPattern = @"ftp:\\\\(.*)";

        private readonly IFileSystem fileSystem;
        private readonly HttpMessageHandler httpMessageHandler;

        public ResourceSource(IFileSystem fileSystem, HttpMessageHandler httpMessageHandler)
        {
            this.fileSystem = fileSystem;
            this.httpMessageHandler = httpMessageHandler;
        }

        public string Acquire(string handle)
        {
            switch (handle)
            {
                case string s when Regex.IsMatch(s, filePattern) :
                    return AcquireFile(handle);
                case string s when Regex.IsMatch(s, httpPattern) :
                    return AcquireHttp(handle);
                case string s when Regex.IsMatch(s, ftpPattern):
                    return AcquireFTP();
                default: throw new NotImplementedException();
            }
        }

        private string AcquireFile(string path)
        {
            var filepath = Regex.Match(path, filePattern).Groups[1].Value;
            return this.fileSystem.File.ReadAllText(filepath);

        }

        private string AcquireHttp(string url)
        {
            HttpClient client = new HttpClient(httpMessageHandler);
            return client.GetStringAsync(url).GetAwaiter().GetResult();
        }

        private string AcquireFTP()
        {
            throw new NotImplementedException();    
        }
    }
}
