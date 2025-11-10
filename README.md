# NETDevRecrutingTest

# Zadanie 1

W tym zadaniu stworzyłem stukturę przechowywującą pracownika, jego przełożonego oraz rząd jakiego jest ten przełożony. Funckja `FillEmployeesStructure` przechodzi po podanych pracownikach i w każdej iteracji dodatkowo po wszystkich przełożonych wzwyż (jeśli jest obecny). Dzięki temu, w łatwy sposób funckja `GetSuperiorRowOfEmployee` za pomocą LINQ, możemy wyszukać dowolną parę pracownika i jego przełożonego wraz z rzędem. Jeśli dana para nie występuję w strukturze, oznacza to, że między nimi nie ma relacji.

Dodatkowo stworzyłem dwa proste testy jednostkowe, upewniające się czy struktura jest poprawnie tworzona oraz czy wyszukiwanie zwraca poprawny wynik.

# Zadanie 2
## a) Zwraca listę wszystkich pracowników z zespołu o nazwie “.NET”, którzy mają co najmniej jeden wniosek urlopowy w 2019 roku.
```sql
SELECT DISTINCT
    e.*
FROM
    Employee as e
    JOIN Team as t
        ON e.teamId = t.id
    JOIN Vacation as v
        ON e.id = v.employeeId
WHERE
    t.name = '.NET' AND
    YEAR(v.dateSince) = 2019
```

## b) Zwraca listę pracowników wraz z liczbą dni urlopowych zużytych w bieżącym roku (za dni zużyte uznajemy wszystkie dni we wnioskach urlopowych które są w całości datą przeszłą).
```sql
SELECT
    e.id,
    e.name,
    vp.grantedDays,
    SUM(
        IF(v.isPartialVacation = 1,
           v.numberOfHours / 8,
           DATEDIFF(
               IF(v.dateUntil > CURRENT_DATE(),
                  CURRENT_DATE() - 1,
                  v.dateUntil),
               v.dateSince
           )
           + 1
        )
    ) as daysUsed
FROM
    Employee as e
    JOIN VacationPackage as vp
        ON e.vacationPackageId = vp.id
    JOIN Vacation as v
        ON v.employeeId = e.id
WHERE
    YEAR(v.dateSince) = YEAR(CURRENT_DATE())
    AND v.dateSince < CURRENT_DATE()
GROUP BY
    e.id;
```

## c) Zwraca listę zespołów w których pracownicy nie złożyli jeszcze żadnego dnia urlopowego w 2019 roku.
```sql
SELECT 
    *
FROM 
    Team AS t
WHERE 
    t.id NOT IN (
        SELECT 
            t2.id
        FROM 
            Employee as e
            JOIN Vacation AS v
                ON e.id = v.employeeId
            JOIN Team AS t2
                ON e.teamId = t2.id
        WHERE 
            YEAR(v.dateSince) = 2019
    );
```