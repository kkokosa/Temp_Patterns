namespace Reports.Main.Bridge.V2.Reports;

internal class DailyReport : BaseReport
{
    public DailyReport(ISalesRepository salesRepository, ISpreadsheetEngine spreadsheetEngine)
        : base(salesRepository, spreadsheetEngine)
    {
    }

    protected override List<List<object>> PrepareData(List<Sale> sales)
    {
        List<List<object>> data = sales
            .GroupBy(s => new { s.Date, s.ProductName })
            .Select(g => new List<object> { g.Key.ProductName, g.Sum(s => s.Amount) })
            .ToList();
        return data;
    }

    protected override void BuildReport(DateOnly date, List<List<object>> data)
    {
        var dayText = date.ToString("yyyyMMdd");
        var title = $"Daily Sales Report - {dayText}";

        _spreadsheetEngine.Initialize();
        _spreadsheetEngine.AddSpreadsheet(title);
        _spreadsheetEngine.AddSheet(dayText);

        _spreadsheetEngine.SetCellValue(0, 0, "Product");
        _spreadsheetEngine.SetCellValue(0, 1, "Amount");
        for (int i = 0; i < data.Count; i++)
        for (int j = 0; j < data[i].Count; j++)
            _spreadsheetEngine.SetCellValue(i + 1, j, data[i][j].ToString());
        _spreadsheetEngine.Save();
        _spreadsheetEngine.Close();
    }
}