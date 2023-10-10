using Castle.Components.DictionaryAdapter;
using System.Collections;

namespace Adapter.Console.Varia
{
    ///////////////////////////////////////////////////////////////////////////////
    // Dictionary Adapter example

    public interface IRequestData
    {
        string Message { get; set; }
        Guid RequestId { get; set; }
    }

    internal class DictionaryAdapter
    {
        public static void Run()
        {
            var dictionary = new Hashtable();
            var factory = new DictionaryAdapterFactory();
            var adapter = factory.GetAdapter<IRequestData>(dictionary);
            dictionary["RequestId"] = Guid.NewGuid();
            dictionary["Message"] = "Hello world!";

            System.Console.WriteLine($"{adapter.RequestId} - {adapter.Message}");
        }
    }
}
