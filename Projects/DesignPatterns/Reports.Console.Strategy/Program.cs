using Reports.Console.Strategy.Repositories;
using Reports.Console.Strategy.Strategies;
using Reports.Main.Strategy.Reports;
using static System.Runtime.InteropServices.JavaScript.JSType;

var date = DateOnly.FromDateTime(DateTime.Now);
var repository = new CsvSalesRepository("Sales.csv");
var engine = new ExcelStrategy();

var report1 = new MonthlyReport(engine);
var from1 = new DateOnly(date.Year, date.Month, 1).ToDateTime(TimeOnly.MinValue);
var to1 = new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
    .ToDateTime(TimeOnly.MaxValue);
var data1 = repository.GetSales(from1, to1);
var dayText1 = date.ToString("yyyyMMdd");
var title1 = $"Daily Sales Report - {dayText1}";
report1.GenerateReport(title1, data1);
