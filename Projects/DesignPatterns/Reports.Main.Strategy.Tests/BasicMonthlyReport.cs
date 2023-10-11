using FluentAssertions;
using Moq;
using Reports.Main.Strategy.Reports;

namespace Reports.Main.Strategy.Tests
{
    public class BasicMonthlyReportTests
    {
        [Fact]
        public void BasicMonthlyReport()
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
            GivenSpreadsheetEngine();

            var report = new MonthlyReport(_engine.Object);
            report.GenerateReport("Title", sales);

            var day1 = DateTime.Parse("2023-10-01");
            var day2 = DateTime.Parse("2023-10-02");
            ThenEngine().Exported("Title", new List<List<object>>()
            {
                new List<object>() { "Date", "Product", "Amount" },
                new List<object>() { day1, "Apple", 100 },
                new List<object>() { day1, "Banana", 450 },
                new List<object>() { day1, "Cherry", 200 },
                new List<object>() { day2, "Cherry", 100 },
                new List<object>() { day2, "Banana", 75 },
                new List<object>() { day2, "Fig", 100 }
            });
        }

        private Mock<ISpreadsheetStrategy> _engine = new Mock<ISpreadsheetStrategy>();
        private Mock<ISpreadsheetStrategy> GivenSpreadsheetEngine() => _engine;
        private EngineAssert ThenEngine() => new EngineAssert(_engine);

        private class EngineAssert
        {
            private readonly Mock<ISpreadsheetStrategy> _mock;

            public EngineAssert(Mock<ISpreadsheetStrategy> mock) => _mock = mock;

            public EngineAssert Exported(string title, List<List<object>> data)
            {
                _mock.Verify(e => e.Export(
                    It.Is<string>(t => t == title),
                    ItExt.IsDeep(data)), Times.Once);
                return this;
            }
        }
    }

    public static class ItExt
    {
        public static T IsDeep<T>(T expected)
        {
            Func<T, bool> validate = actual =>
            {
                actual.Should().BeEquivalentTo(expected);
                return true;
            };
            return Match.Create<T>(s => validate(s));
        }
    }
}