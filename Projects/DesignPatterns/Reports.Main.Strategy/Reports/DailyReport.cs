using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.Main.Strategy.Reports;

internal class DailyReport : BaseReport
{
    public DailyReport(ISpreadsheetStrategy spreadsheetEngine)
        : base(spreadsheetEngine)
    {
    }

    protected override List<List<object>> PrepareData(List<Sale> sales)
    {
        List<List<object>> result = new() { new() { "Product", "Amount" } };
        List<List<object>> data = sales
            .GroupBy(s => new { s.Date, s.ProductName })
            .Select(g => new List<object> { g.Key.ProductName, g.Sum(s => s.Amount) })
            .ToList();
        result.AddRange(data);
        return result;
    }

    protected override void BuildReport(string title, List<List<object>> data)
    {
        _spreadsheetStrategy.Export(title, data);
    }
}