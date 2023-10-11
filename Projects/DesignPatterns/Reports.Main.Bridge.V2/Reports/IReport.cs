namespace Reports.Main.Bridge.V2.Reports;

public interface IReport
{
    void GenerateReport(DateOnly date, List<Sale> sales);
}