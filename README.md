# NETDevRecrutingTest

Maciej Góralczyk

# Zadanie 1

W tym [zadaniu](https://github.com/goralczm/NETDevRecrutingTest/tree/main/Exercise1) stworzyłem stukturę przechowywującą pracownika, jego przełożonego oraz rząd jakiego jest ten przełożony. Funckja `FillEmployeesStructure` przechodzi po podanych pracownikach i w każdej iteracji dodatkowo po wszystkich przełożonych wzwyż (jeśli jest obecny). Dzięki temu, w łatwy sposób funckja `GetSuperiorRowOfEmployee` za pomocą LINQ, możemy wyszukać dowolną parę pracownika i jego przełożonego wraz z rzędem. Jeśli dana para nie występuję w strukturze, oznacza to, że między nimi nie ma relacji.

Dodatkowo stworzyłem dwa proste [testy jednostkowe](https://github.com/goralczm/NETDevRecrutingTest/blob/main/Exercise1.Tests/EmployeeHierarchyTests.cs)., upewniające się czy struktura jest poprawnie tworzona oraz czy wyszukiwanie zwraca poprawny wynik.

# Zadanie 2
## a) Zwraca listę wszystkich pracowników z zespołu o nazwie “.NET”, którzy mają co najmniej jeden wniosek urlopowy w 2019 roku.
```sql
SELECT DISTINCT
    e.id,
    e.name
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

- Dzięki JOIN, jesteśmy pewni, że podani pracownicy mieli przynajmniej jeden wniosek urlopy.

## b) Zwraca listę pracowników wraz z liczbą dni urlopowych zużytych w bieżącym roku (za dni zużyte uznajemy wszystkie dni we wnioskach urlopowych które są w całości datą przeszłą).
```sql
SELECT
    e.id,
    e.name,
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

- W tym przykładzie uwzględniłem niepełne dni urlopowe (`isPartialVacation`) przy załóżeniu, że dzień pracy wynosi 8 godzin. Dzięki temu wynikiem jest liczba zmiennoprzecinkowa np. 0.5 wykorzystanych dni urlopowych.
- Z treści zadania założyłem również, że data dzisiejsza nie jest w pełni datą przeszłą, więc nie jest uwzględniona w obliczeniach. Jeśli system wymagałby liczenia tego dnia, wystarczy usunąć `-1` z 10 linijki zapytania.


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

- Do rozwiązania tego przykładu użyłem podzapytania, które wybiera wszystkie zespoły mające przynajmniej jeden wniosek urlopowy w roku 2019. Finalnie wybieram wszystkie zespoły, które nie były w tym podzapytaniu, czyli nie miały żadnego wniosku urlopowego.

## Zadanie 3 i 4

Klasa [VacationService](https://github.com/goralczm/NETDevRecrutingTest/blob/main/Exercise345/Services/VacationService.cs) zawiera funkcje implementujące logikę zliczania pozostałych dni urlopowych w tym roku oraz funckję określającą czy dany pracownik może jeszcze zgłosić nowy wniosek urlopowy.

Nie byłem pewien szczegółów zadania, czy zliczanie dni ma być tak samo jak w przypadku zadania 2, czy jednak uwzględniając wszystkie (w tym zaplanowe) dni urlopowe oraz kiedy w kiedy dostępne dni urlopowe się odnawiają, więc przyjąłem takie założenia:
1. Zliczamy wszystkie wnioski dni urlopowych w danym roku (również zaplanowane)
2. Dostępne dni urlopowe z `VacationPackage` przypadają na cały rok (od 1 stycznia do 31 grudnia danego roku)

## Zadanie 5

Tutaj znajdują się wymagane 2 proste [testy jednostkowe](https://github.com/goralczm/NETDevRecrutingTest/blob/main/Exercise345.Tests/VacationServiceTests.cs), jednak pokusiłem się o dodanie jeszcze 2 sprawdzających działanie serwisu przy urlopach z datami zmieniającymi rok (sylwester).

## Zadanie 6

Aby zoptymalizować liczbę zapytań do bazy danych przy zadaniu 3, możemy:
1. Wybierać tylko potrzebne nam dane zamiast wszystkich, jeśli potrzebujemy tylko i wyłącznie id pracownika do wykonania obliczeń, to zamiast `e.*` użyjmy `e.id`.
2. Pogrupować zapytanie za pomocą JOIN, dzięki temu zamiast pobierać oddzielnie pracownika, wnioski urlopowe i pakiet urlopowy w 3 oddzielnych zapytaniach, po ich zjoinowaniu wykonujemy tylko 1.
3. Dodanie indeksu na kolumny, po których często filtrujemy np. `Vacation.employeeId` oraz `VacationPackage.year`.
4. Jeśli wiemy, że np. `VacationPackage` zmienia się bardzo rzadko, możemy wewnętrznie lub zewnętrznie cacheować te dane przez pewien czas.
