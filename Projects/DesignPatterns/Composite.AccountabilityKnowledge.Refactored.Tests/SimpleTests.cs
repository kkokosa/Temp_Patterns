using System;
using System.Linq;
using Xunit;

namespace Composite.AccountabilityKnowledge.Refactored.Tests
{
    public class SimpleTests
    {
        [Fact]
        public void Given_ThereAreNoRules_WhenCreatingRelation_Then_ItFails()
        {
            AccountabilityService service = new AccountabilityService();
            Party company = new Party("IBM", PartyType.Company);
            Party division = new Party("Accounting", PartyType.Division);

            var exception = Record.Exception(
                (Action) (() => service.RegisterChild(AccountabilityType.Functional, division, company)));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void Given_ThereIsCorrectRule_WhenCreatingRelation_Then_ItSucceeds()
        {
            AccountabilityService service = new AccountabilityService();
            service.RegisterRule(AccountabilityType.Functional, PartyType.Division, PartyType.Company);

            Party company = new Party("IBM", PartyType.Company);
            Party division = new Party("Accounting", PartyType.Division);

            service.RegisterChild(AccountabilityType.Functional, division, company);

            Assert.Equal(0, company?.AllParents.Count());
            Assert.Equal(1, company?.AllChildren.Count());

            Assert.Equal(1, division?.AllParents.Count());
            Assert.Equal(0, division?.AllChildren.Count());
        }

        [Fact]
        public void Given_ThereIsCorrectRule_WhenCreatingTwoRelations_Then_ItSucceeds()
        {
            AccountabilityService service = new AccountabilityService();
            service.RegisterRule(AccountabilityType.Functional, PartyType.Division, PartyType.Company);

            Party company = new Party("IBM", PartyType.Company);
            Party division1 = new Party("Accounting", PartyType.Division);
            Party division2 = new Party("HR", PartyType.Division);

            service.RegisterChild(AccountabilityType.Functional, division1, company);
            service.RegisterChild(AccountabilityType.Functional, division2, company);

            Assert.Equal(0, company?.AllParents.Count());
            Assert.Equal(2, company?.AllChildren.Count());
            
            Assert.Equal(1, division1?.AllParents.Count());
            Assert.Equal(0, division1?.AllChildren.Count());
            Assert.Equal(1, division2?.AllParents.Count());
            Assert.Equal(0, division2?.AllChildren.Count());
        }

        [Fact]
        public void Given_ThereIsCorrectRule_WhenCreatingThreeRelations_Then_ItSucceeds()
        {
            AccountabilityService service = new AccountabilityService();
            service.RegisterRule(AccountabilityType.Functional, PartyType.Division, PartyType.Company);
            service.RegisterRule(AccountabilityType.Functional, PartyType.Manager, PartyType.Division);

            Party company = new Party("IBM", PartyType.Company);
            Party division1 = new Party("Accounting", PartyType.Division);
            Party division2 = new Party("HR", PartyType.Division);
            Party manager = new Party("Janek", PartyType.Manager);

            service.RegisterChild(AccountabilityType.Functional, division1, company);
            service.RegisterChild(AccountabilityType.Functional, division2, company);
            service.RegisterChild(AccountabilityType.Functional, manager, division2);

            Assert.Equal(0, company?.AllParents.Count());
            Assert.Equal(3, company?.AllChildren.Count());

            Assert.Equal(1, division1?.AllParents.Count());
            Assert.Equal(0, division1?.AllChildren.Count());
            Assert.Equal(1, division2?.AllParents.Count());
            Assert.Equal(1, division2?.AllChildren.Count());
        }

    }
}
