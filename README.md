# NETDevRecrutingTest

# Zadanie 2
## a) Zwraca listę wszystkich pracowników z zespołu o nazwie “.NET”, którzy mają co najmniej jeden wniosek urlopowy w 2019 roku.
```sql
SELECT DISTINCT
    e.*
FROM
    Employee as e
    JOIN
            Team as t
                ON e.teamId = t.id
    JOIN
            Vacation as v
                ON e.id = v.eployeeId
WHERE
    t.name = '.NET' AND
    YEAR(v.dateSince) = 2019
```

## b) Zwraca listę pracowników wraz z liczbą dni urlopowych zużytych w bieżącym roku (za dni zużyte uznajemy wszystkie dni we wnioskach urlopowych które są w całości datą przeszłą).
```sql
SELECT
    e.name,
    SUM(v.dateUntil - v.dateSince) + 1
FROM
    Employee as e
    JOIN VacationPackage as vp
        ON e.vacationPackageId = vp.id
    JOIN Vacation as v
        ON v.employeeId = e.id
WHERE
    YEAR(v.dateSince) = YEAR(NOW())
GROUP BY
    e.name
```

## c) Zwraca listę zespołów w których pracownicy nie złożyli jeszcze żadnego dnia urlopowego w 2019 roku.
```sql
SELECT 
    *
FROM 
    Team AS t
WHERE 
    t.name NOT IN (
        SELECT 
            t2.name
        FROM 
            Employee as e
            JOIN Vacation AS v ON e.id = v.employeeId
            JOIN Team AS t2 ON e.teamId = t2.id
        WHERE 
            YEAR(v.datesince) = 2019
    );
```