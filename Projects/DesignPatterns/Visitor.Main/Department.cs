using System.Drawing;

public class Department : IUnit
{
    public string Name { get; init; } = "Department";
    public List<Team> Teams { get; init; } = new();

    public void Accept(IVisitor visitor)
    {
        visitor.VisitDepartment(this);
    }
}