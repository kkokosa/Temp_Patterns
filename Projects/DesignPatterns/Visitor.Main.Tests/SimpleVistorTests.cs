namespace Visitor.Main.Tests
{
    public class SimpleVistorTests
    {
        [Fact]
        public void GivenEmptyTeam_TestPrinting()
        {
            var shape = new Team() { Name = "Team" };
            var exporter = new PrintVisitor();

            shape.Accept(exporter);

            var result = exporter.Result;
            Assert.Equal("Team: Team with 0 members:\r\n", result);
        }

        [Fact]
        public void GivenEmployee_TestPrinting()
        {
            var employee = new Employee()
            {
                FirstName = "John",
                LastName = "Doe",
                Position = Position.Regular
            };
            var exporter = new PrintVisitor();

            employee.Accept(exporter);

            var result = exporter.Result;
            Assert.Equal("Employee: John Doe (Regular)\r\n", result);
        }

        [Fact]
        public void GivenTeam_TestPrinting()
        {
            var team = new Team()
            {
                Name = "Team",
            };
            team.AddMember(new Employee()
            {
                FirstName = "John",
                LastName = "Doe",
                Position = Position.Regular
            });
            team.AddMember(new Employee()
            {
                FirstName = "John",
                LastName = "Rambo",
                Position = Position.Manager
            });
            var exporter = new PrintVisitor();

            team.Accept(exporter);

            var result = exporter.Result;
            Assert.Equal("Team: Team with 2 members:\r\n" +
                         "Employee: John Doe (Regular)\r\n" +
                         "Employee: John Rambo (Manager)\r\n", result);
        }
    }
}