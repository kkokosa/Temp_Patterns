namespace Reports.Main.Bridge.Reports;

public interface IReport
{
    void GenerateReport(DateOnly date);
}