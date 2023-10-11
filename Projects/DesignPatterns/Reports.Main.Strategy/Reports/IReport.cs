namespace Reports.Main.Strategy.Reports;

public interface IReport
{
    void GenerateReport(string title, List<Sale> sales);
}