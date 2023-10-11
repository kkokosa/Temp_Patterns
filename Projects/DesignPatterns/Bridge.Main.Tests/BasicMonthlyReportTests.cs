using Moq;
using Reports.Main.Bridge;
using Reports.Main.Bridge.Reports;


namespace Reports.Main.Tests
{
    public class BasicMonthlyReportTests
    {
        [Fact]
        public void BasicMonthlyReport()
        {
            var day1 = DateTime.Parse("2023-10-01");
            var day2 = DateTime.Parse("2023-10-02");
            var repository = new StubRepository();
            var engine = new Mock<ISpreadsheetEngine>();
            var report = new MonthlyReport(repository, engine.Object);

            report.GenerateReport(DateOnly.Parse("2023-10"));

            engine.Verify(e => e.Initialize(), Times.Once);
            engine.Verify(e => e.AddSpreadsheet("MonthlySalesReport-202310"), Times.Once);
            engine.Verify(e => e.AddSheet("202310"), Times.Once);

            engine.Verify(e => e.SetCellValue(1, 1, "Date"), Times.Once);
            engine.Verify(e => e.SetCellValue(1, 2, "Product"), Times.Once);
            engine.Verify(e => e.SetCellValue(1, 3, "Amount"), Times.Once);

            engine.Verify(e => e.SetCellValue(2, 1, day1), Times.Once);
            engine.Verify(e => e.SetCellValue(2, 2, "Apple"), Times.Once);
            engine.Verify(e => e.SetCellValue(2, 3, 100), Times.Once);

            engine.Verify(e => e.SetCellValue(3, 1, day1), Times.Once);
            engine.Verify(e => e.SetCellValue(3, 2, "Banana"), Times.Once);
            engine.Verify(e => e.SetCellValue(3, 3, 450), Times.Once);

            engine.Verify(e => e.SetCellValue(4, 1, day1), Times.Once);
            engine.Verify(e => e.SetCellValue(4, 2, "Cherry"), Times.Once);
            engine.Verify(e => e.SetCellValue(4, 3, 200), Times.Once);

            engine.Verify(e => e.SetCellValue(5, 1, day2), Times.Once);
            engine.Verify(e => e.SetCellValue(5, 2, "Cherry"), Times.Once);
            engine.Verify(e => e.SetCellValue(5, 3, 100), Times.Once);

            engine.Verify(e => e.SetCellValue(6, 1, day2), Times.Once);
            engine.Verify(e => e.SetCellValue(6, 2, "Banana"), Times.Once);
            engine.Verify(e => e.SetCellValue(6, 3, 75), Times.Once);

            engine.Verify(e => e.SetCellValue(7, 1, day2), Times.Once);
            engine.Verify(e => e.SetCellValue(7, 2, "Fig"), Times.Once);
            engine.Verify(e => e.SetCellValue(7, 3, 100), Times.Once);
            engine.Verify(e => e.Save(), Times.Once);
            engine.Verify(e => e.Close(), Times.Once);
        }

        internal class StubRepository : ISalesRepository
        {
            public List<Sale> GetSales(DateTime from, DateTime to)
            {
                List<Sale> sales = new List<Sale>
                {
                    new Sale(1, DateTime.Parse("2023-10-01"), "Apple", 100, 1.2m),
                    new Sale(2, DateTime.Parse("2023-10-01"), "Banana", 150, 0.8m),
                    new Sale(3, DateTime.Parse("2023-10-01"), "Banana", 150, 0.8m),
                    new Sale(4, DateTime.Parse("2023-10-01"), "Banana", 150, 0.8m),
                    new Sale(5, DateTime.Parse("2023-10-01"), "Cherry", 200, 2.5m),
                    new Sale(6, DateTime.Parse("2023-10-02"), "Cherry", 50, 3.0m),
                    new Sale(7, DateTime.Parse("2023-10-02"), "Cherry", 50, 3.0m),
                    new Sale(8, DateTime.Parse("2023-10-02"), "Banana", 75, 4.5m),
                    new Sale(9, DateTime.Parse("2023-10-02"), "Fig", 100, 1.5m)
                };
                return sales;
            }
        }
    }
}