using System.IO.Compression;

string path = @"secret.txt.gz";

// Stream Decorator

// Create a FileStream (ConcreteComponent) for the file we will read
using (var fileStream = new FileStream(path, FileMode.Open))
{
    // Decorate it with a BufferedStream (ConcreteDecorator) for better performance
    using (var bufferedStream = new BufferedStream(fileStream))
    {
        // Decorate it with a GzipStream (ConcreteDecorator) to decompress the data
        using (var gzipStream = new GZipStream(bufferedStream, CompressionMode.Decompress))
        {
            using (var reader = new StreamReader(gzipStream))
            {
                string text = reader.ReadToEnd();
                Console.WriteLine(text);
            }
        }
    }
}

// In decorators order is important and may change behaviour
using (var fileStream = new FileStream(path, FileMode.Open))
{
    using var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
    using var bufferedStream = new BufferedStream(gzipStream);
    using var reader = new StreamReader(bufferedStream);
    string text = reader.ReadToEnd();
    Console.WriteLine(text);
}



