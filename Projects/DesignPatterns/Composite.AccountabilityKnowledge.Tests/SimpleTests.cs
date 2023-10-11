using System;
using System.Linq;
using Xunit;

namespace Composite.AccountabilityKnowledge.Tests
{
    public class SimpleTests
    {
        // Most probably fails - why?
        [Fact]
        public void Given_ThereAreNoRules_WhenCreatingRelation_Then_ItFails()
        {
            Party company = new Party("IBM", PartyType.Company);
            Party division = new Party("Accounting", PartyType.Division);

            Accountability accountability;
            var exception = Record.Exception(
                () => accountability = new Accountability(AccountabilityType.Functional, division, company));

            Assert.NotNull(exception);
            Assert.IsType<InvalidOperationException>(exception);
        }

        [Fact]
        public void Given_ThereIsCorrectRule_WhenCreatingRelation_Then_ItSucceeds()
        {
            Accountability.RegisterRule(AccountabilityType.Functional, PartyType.Division, PartyType.Company);

            Party company = new Organization("IBM", PartyType.Company);
            Party division = new Organization("Accounting", PartyType.Division);

            Accountability accountability = new Accountability(AccountabilityType.Functional, division, company);

            Assert.NotNull(accountability);
            Assert.Equal(AccountabilityType.Functional, accountability.Type);
            Assert.Equal(accountability.Parent, company);
            Assert.Equal(accountability.Child, division);

            Assert.Equal(0, company?.AllParents.Count());
            Assert.Equal(1, company?.AllChildren.Count());

            Assert.Equal(1, division?.AllParents.Count());
            Assert.Equal(0, division?.AllChildren.Count());
        }

        [Fact]
        public void Given_ThereIsCorrectRule_WhenCreatingTwoRelations_Then_ItSucceeds()
        {
            Accountability.RegisterRule(AccountabilityType.Functional, PartyType.Division, PartyType.Company);

            Party company = new Organization("IBM", PartyType.Company);
            Party division1 = new Organization("Accounting", PartyType.Division);
            Party division2 = new Organization("HR", PartyType.Division);

            Accountability accountability1 = new Accountability(AccountabilityType.Functional, division1, company);
            Accountability accountability2 = new Accountability(AccountabilityType.Functional, division2, company);

            Assert.NotNull(accountability1);
            Assert.Equal(AccountabilityType.Functional, accountability1.Type);
            Assert.Equal(accountability1.Parent, company);
            Assert.Equal(accountability1.Child, division1);

            Assert.NotNull(accountability2);
            Assert.Equal(AccountabilityType.Functional, accountability2.Type);
            Assert.Equal(accountability2.Parent, company);
            Assert.Equal(accountability2.Child, division2);

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
            Accountability.RegisterRule(AccountabilityType.Functional, PartyType.Division, PartyType.Company);
            Accountability.RegisterRule(AccountabilityType.Functional, PartyType.Manager, PartyType.Division);

            Party company = new Organization("IBM", PartyType.Company);
            Party division1 = new Organization("Accounting", PartyType.Division);
            Party division2 = new Organization("HR", PartyType.Division);
            Party manager = new Party("Janek", PartyType.Manager);

            Accountability accountability1 = new Accountability(AccountabilityType.Functional, division1, company);
            Accountability accountability2 = new Accountability(AccountabilityType.Functional, division2, company);
            Accountability accountability3 = new Accountability(AccountabilityType.Functional, manager, division2);

            Assert.NotNull(accountability1);
            Assert.NotNull(accountability2);
            Assert.NotNull(accountability3);

            Assert.Equal(0, company?.AllParents.Count());
            Assert.Equal(3, company?.AllChildren.Count());

            Assert.Equal(1, division1?.AllParents.Count());
            Assert.Equal(0, division1?.AllChildren.Count());
            Assert.Equal(1, division2?.AllParents.Count());
            Assert.Equal(1, division2?.AllChildren.Count());
        }

    }
}
