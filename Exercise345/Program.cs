using Exercise345.Models;
using Exercise345.Services;

var vacationPackage = new VacationPackage()
{
    Id = 0,
    Name = "Test",
    GrantedDays = 28,
    Year = DateTime.Now.Year,
};

var team = new Team()
{
    Id = 0,
    Name = ".NET",
};

var employee = new Employee()
{
    Id = 0,
    Name = "Jan Kowalski",
    TeamId = team.Id,
    VacationPackageId = vacationPackage.Id,
};

var vacations = new List<Vacation>()
{
    new()
    {
        Id = 0,
        DateSince = DateTime.Now.AddDays(-5),
        DateUntil = DateTime.Now,
        EmployeeId = employee.Id,
        IsPartialVacation = 0,
        NumberOfHours = 0,
    }
};

var vacationService = new VacationService();

Console.WriteLine($"Jan Kowalski ma {vacationService.CountFreeDaysForEmployee(employee, vacations, vacationPackage)} dni urlopu pozostałych do wykorzystania w tym roku.");