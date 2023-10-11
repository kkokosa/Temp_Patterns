using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Reports.Main.Bridge;
using Reports.Main.Bridge.Reports;

namespace Reports.Main.Tests
{
    public class ImprovedMonthlyReportTests
    {
        [Fact]
        public void BasicMonthlyReport()
        {
            var repository = new StubRepository();
            GivenSpreadsheetEngine();

            var report = new MonthlyReport(repository, _engine.Object);
            report.GenerateReport(DateOnly.Parse("2023-10"));

            var day1 = DateTime.Parse("2023-10-01");
            var day2 = DateTime.Parse("2023-10-02");
            ThenEngine().Initialized()
                .SpreadsheetAdded("MonthlySalesReport-202310")
                .SheetAdded("202310")
                .RowAdded(1, "Date", "Product", "Amount")
                .RowAdded(2, day1, "Apple", 100)
                .RowAdded(3, day1, "Banana", 450)
                .RowAdded(4, day1, "Cherry", 200)
                .RowAdded(5, day2, "Cherry", 100)
                .RowAdded(6, day2, "Banana", 75)
                .RowAdded(7, day2, "Fig", 100)
                .SavedAndClosed();
        }

        private Mock<ISpreadsheetEngine> _engine = new Mock<ISpreadsheetEngine>();
        private Mock<ISpreadsheetEngine> GivenSpreadsheetEngine() => _engine;
        private EngineAssert ThenEngine() => new EngineAssert(_engine);

        private class EngineAssert
        {
            private readonly Mock<ISpreadsheetEngine> _mock;

            public EngineAssert(Mock<ISpreadsheetEngine> mock) => _mock = mock;

            public EngineAssert Initialized()
            {
                _mock.Verify(e => e.Initialize(), Times.Once); 
                return this;
            }

            public EngineAssert SpreadsheetAdded(string title)
            {
                _mock.Verify(e => e.AddSpreadsheet(title), Times.Once); 
                return this;
            }

            public EngineAssert SheetAdded(string title)
            {
                _mock.Verify(e => e.AddSheet(title), Times.Once); 
                return this;
            }

            public EngineAssert RowAdded(int row, params object[] values)
            {
                for (int i = 0; i < values.Length; i++)
                    _mock.Verify(e => e.SetCellValue(row, i+1, values[i]), Times.Once);
                return this;
            }

            public EngineAssert SavedAndClosed()
            {
                _mock.Verify(e => e.Save(), Times.Once); 
                _mock.Verify(e => e.Close(), Times.Once); 
                return this;
            }
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
