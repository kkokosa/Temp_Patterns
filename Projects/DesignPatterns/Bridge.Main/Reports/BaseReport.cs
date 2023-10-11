namespace Reports.Main.Bridge.Reports;

public abstract class BaseReport : IReport
{
    protected ISalesRepository _salesRepository;
    protected ISpreadsheetEngine _spreadsheetEngine;

    protected BaseReport(ISalesRepository salesRepository, ISpreadsheetEngine spreadsheetEngine)
    {
        _salesRepository = salesRepository;
        _spreadsheetEngine = spreadsheetEngine;
    }

    public virtual void GenerateReport(DateOnly date)
    {
        var sales = GetSales(date);
        var data = PrepareData(sales);
        BuildReport(date, data);
    }

    protected abstract List<Sale> GetSales(DateOnly date);
    protected abstract List<List<object>> PrepareData(List<Sale> sales);
    protected abstract void BuildReport(DateOnly dateOnly, List<List<object>> data);
}