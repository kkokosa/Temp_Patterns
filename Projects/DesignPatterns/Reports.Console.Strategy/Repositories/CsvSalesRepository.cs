using CsvHelper;
using System.Globalization;
using Reports.Main.Strategy;

namespace Reports.Console.Strategy.Repositories;

internal class CsvSalesRepository : ISalesRepository
{
    private string _path;

    public CsvSalesRepository(string path)
    {
        _path = path;
    }

    public List<Sale> GetSales(DateTime from, DateTime to)
    {
        using var reader = new StreamReader(_path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Sale>();
        return records.Where(x => x.Date >= from && x.Date <= to).ToList();
    }
}