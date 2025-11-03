using Exercise1.Models;

namespace Exercise1.Services
{
    public class EmployeeHierarchyService
    {
        private List<EmployeeStructure> _employeesHierarchyCache = new();

        public EmployeeHierarchyService() { }

        public EmployeeHierarchyService(List<Employee> employees)
        {
            _employeesHierarchyCache = FillEmployeesStructure(employees);
        }

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
