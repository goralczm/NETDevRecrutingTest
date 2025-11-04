using Exercise3.Models;
using Exercise3.Services;

namespace Exercise3.Tests
{
    public class VacationServiceTests
    {
        private VacationPackage CreateSimpleVacationPackage(int grantedDays)
        {
            return new()
            {
                Id = 0,
                Name = "Test",
                GrantedDays = grantedDays,
                Year = DateTime.Now.Year,
            };
        }

        private List<Vacation> CreateVacationsForUser(Employee employee, int usedDays)
        {
            return new()
            {
                new()
                {
                    Id = 0,
                    DateSince = DateTime.Now.AddDays(-usedDays),
                    DateUntil = DateTime.Now,
                    EmployeeId = employee.Id,
                    IsPartialVacation = 0,
                    NumberOfHours = 0,
                }
            };
        }

        private Employee CreateSimpleEmployee()
        {
            return new()
            {
                Id = 0,
                Name = "Jan Kowalski",
                TeamId = 0,
                VacationPackageId = 0,
            };
        }

        [Fact]
        public void employee_can_request_vaction()
        {
            var employee = CreateSimpleEmployee();
            var vacations = new List<Vacation>();
            var vacationPackage = CreateSimpleVacationPackage(grantedDays: 5);

            var vacationService = new VacationService();

            Assert.True(vacationService.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage));
        }

        [Fact]
        public void employee_cant_request_vacation()
        {
            var employee = CreateSimpleEmployee();
            var vacations = CreateVacationsForUser(employee, usedDays: 5);
            var vacationPackage = CreateSimpleVacationPackage(grantedDays: 5);

            var vacationService = new VacationService();

            Assert.False(vacationService.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage));
        }
    }
}
