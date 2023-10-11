using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Console
{
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
}
