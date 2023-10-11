public class Team : IUnit
{
    private readonly List<Employee> members = new();
    public string Name { get; set; }

    public IReadOnlyCollection<Employee> Members => members;
    public Employee Leader 
        => members.SingleOrDefault(x => x.Position == Position.Manager);

    public void AddMember(Employee employee)
    {
        members.Add(employee);
    }

    public void Accept(IVisitor visitor)
    {
        visitor.VisitTeam(this);
    }
}