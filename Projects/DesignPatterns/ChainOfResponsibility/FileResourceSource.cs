using System.IO;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace ChainOfResponsibility
{
    public class FileResourceSource : ResourceSource
    {
        private readonly IFileSystem fileSystem;

        public FileResourceSource(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        private const string pattern = @"file:\\\\(.*)";
        protected override bool CanHandle(string handle) 
            => Regex.IsMatch(handle, pattern);

        protected override string InternalAcquire(string handle)
        {
            var filepath = Regex.Match(handle, pattern).Groups[1].Value;
            return this.fileSystem.File.ReadAllText(filepath);
        }
    }
}