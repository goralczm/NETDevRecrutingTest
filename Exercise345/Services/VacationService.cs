using Exercise345.Models;

namespace Exercise345.Services
{
    public class VacationService
    {
        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            if (vacations == null)
                throw new ArgumentNullException(nameof(vacations));

            if (vacationPackage == null)
                throw new ArgumentNullException(nameof(vacationPackage));

            var currentYear = DateTime.Now.Year;

            var usedDays = vacations
                .Where(v => v.EmployeeId == employee.Id && (v.DateSince.Year == currentYear || v.DateUntil.Year == currentYear))
                .Sum(v => (ClampDateToYear(v.DateUntil, currentYear) - ClampDateToYear(v.DateSince, currentYear)).Days + 1);

            var remainingDays = vacationPackage.GrantedDays - usedDays;

            return Math.Max(remainingDays, 0);
        }

        private DateTime ClampDateToYear(DateTime date, int year)
        {
            if (date.Year < year)
                return new DateTime(year, 1, 1);
            
            if (date.Year > year)
                return new DateTime(year, 12, 31);

            return date;
        }

        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            var remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);

            return remainingDays > 0;
        }
    }
}
