public interface IVisitor
{
    void VisitDepartment(Department department);
    void VisitTeam(Team team);
    void VisitEmployee(Employee employee);
}