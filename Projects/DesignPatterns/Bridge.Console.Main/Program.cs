using Reports.Console.Bridge.Engines;
using Reports.Console.Bridge.Repositories;
using Reports.Main.Bridge.Reports;

var now = DateOnly.FromDateTime(DateTime.Now);
var repository = new CsvSalesRepository("Sales.csv");
var engine = new ExcelEngine();

var report = new MonthlyReport(repository, engine);
report.GenerateReport(now);

var report2 = new MonthlyReport(repository, engine);
report2.GenerateReport(now);