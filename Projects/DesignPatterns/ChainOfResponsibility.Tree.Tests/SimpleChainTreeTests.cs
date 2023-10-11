using System;
using System.Collections.Generic;
using Xunit;

namespace ChainOfResponsibility.Tree.Tests
{
    public class SimpleChainTreeTests
    {
        [Fact]
        public void GivenOnlyManualPrinterAvailable_WhenOrderIssued_ShouldUseManualPrinter()
        {
            var expectedGuid = Guid.NewGuid();
            PrintingUnit company = new PrintingUnit(Guid.NewGuid(), "Drukarnia Janusz",
                new List<IPrintHandler>()
                {
                    new PrintingFarm(Guid.NewGuid(), new List<SinglePrinter>()
                    {
                        new SinglePrinter(Guid.NewGuid(), "A", SinglePrinter.State.Busy),
                        new SinglePrinter(Guid.NewGuid(), "B", SinglePrinter.State.Queued)
                    }),
                    new ManualPrinter(expectedGuid, new Employee() { Name = "Janusz", IsOnDuty = true})
                });

            PrintOrder order = new PrintOrder(Guid.NewGuid());

            var actualGuid = company.Print(order);

            Assert.Equal(expectedGuid, actualGuid);
        }

        [Fact]
        public void GivenOnlySinglePrinterAvailable_WhenOrderIssued_ShouldUseThatPrinter()
        {
            var expectedGuid = Guid.NewGuid();
            PrintingUnit company = new PrintingUnit(Guid.NewGuid(), "Drukarnia Janusz",
                new List<IPrintHandler>()
                {
                    new PrintingFarm(Guid.NewGuid(), new List<SinglePrinter>()
                    {
                        new SinglePrinter(expectedGuid, "A", SinglePrinter.State.Waiting),
                        new SinglePrinter(Guid.NewGuid(), "B", SinglePrinter.State.Queued)
                    }),
                    new ManualPrinter(Guid.NewGuid(), new Employee() { Name = "Janusz", IsOnDuty = false})
                });

            PrintOrder order = new PrintOrder(Guid.NewGuid());

            var actualGuid = company.Print(order);

            Assert.Equal(expectedGuid, actualGuid);

        }
    }
}
