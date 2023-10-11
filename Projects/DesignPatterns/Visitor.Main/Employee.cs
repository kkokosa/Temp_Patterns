using Visitor.SourceGenerators;

[VisitableBy<IVisitor>]
public partial class Employee : IUnit
{
    public string Name => $"{FirstName} {LastName}";
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public Position Position { get; set; }
    public DateTime HireDate { get; set; }
    public Money Salary { get; set; }
    public Team Team { get; set; }
}

public enum Position
{
    Regular,
    Manager
}

