namespace Reports.Main.Bridge.V2.Reports;

public class MonthlyReport : BaseReport
{
    public MonthlyReport(ISalesRepository salesRepository, ISpreadsheetEngine spreadsheetEngine)
        : base(salesRepository, spreadsheetEngine)
    {
    }

    protected override List<List<object>> PrepareData(List<Sale> sales)
    {
        var data = sales
            .GroupBy(s => new { s.Date, s.ProductName })
            .Select(g => new List<object> { g.Key.Date, g.Key.ProductName, g.Sum(s => s.Amount) })
            .ToList();
        return data;
    }

    protected override void BuildReport(DateOnly date, List<List<object>> data)
    {
        var monthText = date.ToString("yyyyMM");

        List<string> headers = new List<string> { "Date", "Product", "Amount" };

        _spreadsheetEngine.Initialize();
        _spreadsheetEngine.AddSpreadsheet(@$"MonthlySalesReport-{monthText}");
        _spreadsheetEngine.AddSheet(monthText);
        for (int i = 0; i < headers.Count; i++)
            _spreadsheetEngine.SetCellValue(1, i + 1, headers[i]);
        for (int i = 0; i < data.Count; i++)
        for (int j = 0; j < data[i].Count; j++)
            _spreadsheetEngine.SetCellValue(i + 2, j + 1, data[i][j]);
        _spreadsheetEngine.Save();
        _spreadsheetEngine.Close();
    }
}