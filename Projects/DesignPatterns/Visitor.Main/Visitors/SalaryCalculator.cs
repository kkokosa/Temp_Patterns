using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Main.Visitors
{
    internal class SalaryCalculator : IVisitor
    {
        private decimal totalSalary;
        public void VisitDepartment(Department department)
        {
            foreach (var team in department.Teams)
            {
                VisitTeam(team);
            }
        }

        public void VisitTeam(Team team)
        {
            foreach (var member in team.Members)
            {
                VisitEmployee(member);
            }
        }

        public void VisitEmployee(Employee employee)
        {
            totalSalary += employee.Salary.Amount; 
        }
    }
}
