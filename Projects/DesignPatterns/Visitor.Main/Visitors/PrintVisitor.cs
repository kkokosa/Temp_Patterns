using System;
using System.Text;

public class PrintVisitor : IVisitor
{
    private StringBuilder _result = new StringBuilder();
    public void VisitDepartment(Department department)
    {
        _result.AppendLine($"Department: {department.Name}:");
        foreach (var team in department.Teams)
        {
            team.Accept(this);
        }
    }

    public void VisitTeam(Team team)
    {
        _result.AppendLine($"Team: {team.Name} with {team.Members.Count} members:");
        foreach (var member in team.Members)
        {
            member.Accept(this);
        }
    }

    public void VisitEmployee(Employee employee)
    {
        _result.AppendLine($"Employee: {employee.Name} ({employee.Position})");
    }

    public string? Result => _result.ToString();
}



