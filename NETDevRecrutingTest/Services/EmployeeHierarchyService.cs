using NETDevRecrutingTest.Models;

namespace NETDevRecrutingTest.Services
{
    internal class EmployeeHierarchyService
    {
        private List<EmployeeStructure> _employeesHierarchyCache = new();

        public List<EmployeeStructure> FillEmployeesStructure(List<Employee> employees)
        {
            List<EmployeeStructure> structures = new();

            foreach (Employee employee in employees)
            {
                if (employee.Superior == null)
                    continue;

                Employee current = employee.Superior;
                int level = 1;

                while (current != null)
                {
                    structures.Add(new()
                    {
                        EmployeeId = employee.Id,
                        SuperiorId = current.Id,
                        Level = level,
                    });

                    current = current.Superior;
                    level++;
                }
            }

            _employeesHierarchyCache = structures;
            return structures;
        }

        public int? GetSuperiorRowOfEmployee(int employeeId, int superiorId)
        {
            return _employeesHierarchyCache
                .FirstOrDefault(es => es.EmployeeId == employeeId && es.SuperiorId == superiorId)
                ?.Level;
        }
    }
}
