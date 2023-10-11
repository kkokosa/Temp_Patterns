namespace Reports.Main.Strategy.Reports;

public class MonthlyReport : BaseReport
{
    public MonthlyReport(ISpreadsheetStrategy spreadsheetEngine)
        : base(spreadsheetEngine)
    {
    }

    protected override List<List<object>> PrepareData(List<Sale> sales)
    {
        List<List<object>> result = new() { new() { "Date", "Product", "Amount" } };
        var data = sales
            .GroupBy(s => new { s.Date, s.ProductName })
            .Select(g => new List<object> { g.Key.Date, g.Key.ProductName, g.Sum(s => s.Amount) })
            .ToList();
        result.AddRange(data);
        return result;
    }

    protected override void BuildReport(string title, List<List<object>> data)
    {
        _spreadsheetStrategy.Export(title, data);
    }
}