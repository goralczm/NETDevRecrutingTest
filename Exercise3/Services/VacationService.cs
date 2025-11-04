using Exercise3.Models;

namespace Exercise3.Services
{
    public class VacationService
    {
        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            var currentYear = DateTime.Now.Year;

            var usedDays = vacations
                .Where(v => v.EmployeeId == employee.Id && v.DateUntil.Year == currentYear)
                .Sum(v => (v.DateUntil - v.DateSince).Days + 1);

            var remainingDays = vacationPackage.GrantedDays - usedDays;

            return Math.Max(remainingDays, 0);
        }

        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            var remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);

            return remainingDays > 0;
        }
    }
}
