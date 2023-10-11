using OfficeOpenXml;

namespace Reports.Console
{
    public class MonthlyReport : IReport
    {
        private readonly ISalesRepository salesRepository;

        public MonthlyReport(ISalesRepository salesRepository)
        {
            this.salesRepository = salesRepository;
        }

        public void GenerateReport(DateOnly monthDate)
        {
            var monthText = monthDate.ToString("yyyyMM");
            
            // Get sales data for a specified month
            var sales = salesRepository.GetSales(
                new DateOnly(monthDate.Year, monthDate.Month, 1).ToDateTime(TimeOnly.MinValue),
                new DateOnly(monthDate.Year, monthDate.Month, DateTime.DaysInMonth(monthDate.Year, monthDate.Month)).ToDateTime(TimeOnly.MaxValue));

            var data = sales
                .GroupBy(s => new { s.Date, s.ProductName })
                .Select(g => new List<object> { g.Key.Date, g.Key.ProductName, g.Sum(s => s.Amount) })
                .ToList();

            List<string> headers = new List<string> { "Date", "Product", "Amount" };

            // Create Excel package
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excel = new ExcelPackage())
            {
                // Add worksheet
                var ws = excel.Workbook.Worksheets.Add(monthText);

                // Add headers
                for (int i = 0; i < headers.Count; i++)
                {
                    ws.Cells[1, i + 1].Value = headers[i];
                }

                // Add data
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Count; j++)
                    {
                        ws.Cells[i + 2, j + 1].Value = data[i][j];
                    }
                }

                // Save Excel file
                FileInfo excelFile = new FileInfo(@$"MonthlySalesReport-{monthText}.xlsx");
                excel.SaveAs(excelFile);
            }
        }
    }
}
