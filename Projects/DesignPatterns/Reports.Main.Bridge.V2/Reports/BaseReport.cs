namespace Reports.Main.Bridge.V2.Reports;

public abstract class BaseReport : IReport
{
    protected ISalesRepository _salesRepository;
    protected ISpreadsheetEngine _spreadsheetEngine;

    protected BaseReport(ISalesRepository salesRepository, ISpreadsheetEngine spreadsheetEngine)
    {
        _salesRepository = salesRepository;
        _spreadsheetEngine = spreadsheetEngine;
    }

    public virtual void GenerateReport(DateOnly date, List<Sale> sales)
    {
        var data = PrepareData(sales);
        BuildReport(date, data);
    }

    protected abstract List<List<object>> PrepareData(List<Sale> sales);
    protected abstract void BuildReport(DateOnly dateOnly, List<List<object>> data);
}