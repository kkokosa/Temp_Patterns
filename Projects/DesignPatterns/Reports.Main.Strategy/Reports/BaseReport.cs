namespace Reports.Main.Strategy.Reports;

public abstract class BaseReport : IReport
{
    protected ISpreadsheetStrategy _spreadsheetStrategy;

    protected BaseReport(ISpreadsheetStrategy spreadsheetStrategy)
    {
        _spreadsheetStrategy = spreadsheetStrategy;
    }

    public virtual void GenerateReport(string title, List<Sale> sales)
    {
        var data = PrepareData(sales);
        BuildReport(title, data);
    }

    protected abstract List<List<object>> PrepareData(List<Sale> sales);
    protected abstract void BuildReport(string title, List<List<object>> data);
}