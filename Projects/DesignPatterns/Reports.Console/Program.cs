using Reports.Console;

var now = DateOnly.FromDateTime(DateTime.Now);
var repository = new CsvSalesRepository("Sales.csv");

var report1 = new DailyReport(repository);
report1.GenerateReport(now);

var report2 = new MonthlyReport(repository);
report2.GenerateReport(now);