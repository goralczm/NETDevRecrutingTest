using Exercise1.Models;
using Exercise1.Services;

var janKowalski = new Employee
{
    Id = 1,
    Name = "Jan Kowalski",
    SuperiorId = null,
    Superior = null,
};

var kamilNowak = new Employee
{
    Id = 2,
    Name = "Kamil Nowak",
    SuperiorId = 1,
    Superior = janKowalski,
};

var employees = new List<Employee>
{
    janKowalski,
    kamilNowak,
};

var employeeHierarchyService = new EmployeeHierarchyService(employees);

Console.WriteLine($"Jan Kowalski jest przełożonym Kamila Nowaka rzędu {employeeHierarchyService.GetSuperiorRowOfEmployee(kamilNowak.Id, janKowalski.Id)}");
