using Exercise1.Models;
using Exercise1.Services;

namespace Exercise1.Tests
{
    public class EmployeeHierarchyTests
    {
        private List<Employee> CreateSimpleEmployeesList()
        {
            Employee janKowalski = new Employee
            {
                Id = 1,
                Name = "Jan Kowalski",
                SuperiorId = null,
                Superior = null,
            };

            Employee kamilNowak = new Employee
            {
                Id = 2,
                Name = "Kamil Nowak",
                SuperiorId = 1,
                Superior = janKowalski,
            };

            Employee annaMariacka = new Employee
            {
                Id = 3,
                Name = "Anna Mariacka",
                SuperiorId = 1,
                Superior = janKowalski,
            };

            Employee andrzejAbacki = new Employee
            {
                Id = 4,
                Name = "Andrzej Abacki",
                SuperiorId = 2,
                Superior = kamilNowak,
            };

            return new()
            {
                janKowalski,
                kamilNowak,
                annaMariacka,
                andrzejAbacki,
            };
        }

        [Fact]
        public void FillEmployeeStructure_AssignsCorrectSuperiorAndLevel()
        {
            var employees = CreateSimpleEmployeesList();
            var service = new EmployeeHierarchyService();

            var employeeStructure = service.FillEmployeesStructure(employees);

            Assert.Contains(employeeStructure, es => es.EmployeeId == 2 && es.SuperiorId == 1 && es.Level == 1);
            Assert.Contains(employeeStructure, es => es.EmployeeId == 3 && es.SuperiorId == 1 && es.Level == 1);
        }

        [Fact]
        public void GetSuperiorRowOfEmployee_ReturnsExpectedSuperiorLevel()
        {
            var service = new EmployeeHierarchyService(CreateSimpleEmployeesList());

            Assert.Null(service.GetSuperiorRowOfEmployee(1, 5));
            Assert.Equal(service.GetSuperiorRowOfEmployee(2, 1), 1);    
            Assert.Equal(service.GetSuperiorRowOfEmployee(3, 1), 1);
            Assert.Equal(service.GetSuperiorRowOfEmployee(4, 2), 1);
            Assert.Equal(service.GetSuperiorRowOfEmployee(4, 1), 2);
        }
    }
}
