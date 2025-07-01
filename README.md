# Start apki:

Z katalogu głównego projektu uruchom polecenie:

```bash
docker-compose down --volumes
docker-compose up --build
```

!Jeżeli up nie zadziała, spróbuj:

```bash
dotnet build
```

Następnie otwórz przeglądarkę i przejdź do `http://localhost:5001/swagger/index.html`, aby zobaczyć dokumentację API.

# Uruchomienie testów

```bash
dotnet build
./test.sh
```

# Połączenie frontendu z backendem

Aby połączyć frontend z backendem, upewnij się, że w pliku konfiguracyjnym frontendu (np. `src/config.js` lub `src/constants.js`) masz ustawiony adres URL API:

```javascript
export const API_URL = 'http://localhost:5001/api';
```

# Dodawanie DTO

Jeżeli chcesz dodać tabelę do bazy danych, to przejdź do kolejnego punktu `Migracje`, gdzie opisane jest, jak dodać nowy model i migrację.

DTO (Data Transfer Object) mają służyć do przesyłania danych między warstwami aplikacji, np. między kontrolerami a usługami. DTO powinny być proste i zawierać tylko te pola, które są niezbędne do komunikacji.
DTO powinny znajdować się w katalogu `Api/DTOs`.


# Migracje

## Dodawanie modelu do aplikacji/tabeli do bazy danych

UWAGA: Modele przechowujemy w Domain/Entities!!! Enumy i interfejsy również w Domain

Aby dodać nowy model do aplikacji i utworzyć odpowiadającą mu tabelę w bazie danych, wykonaj następujące kroki:

1. **Dodaj nowy model**: Utwórz nową klasę w katalogu `Domain/Entities`, która będzie reprezentować tabelę w bazie danych. Na przykład:

```csharp
public class NowyModel
{
    public int Id { get; set; }
    public string Nazwa { get; set; }
}
```

2. **Zaktualizuj kontekst bazy danych**: Dodaj nowy model do klasy `AppDbContext` w katalogu `Infrastructure/Data/AppDbContext.cs`:

```csharp
public DbSet<NowyModel> NoweModele { get; set; }
```

3. **Utwórz migrację**: W terminalu, w katalogu głównym projektu, uruchom polecenie:
```bash
dotnet ef migrations add TestoweWywolanie --project Infrastructure --startup-project Api
dotnet ef database update --project Infrastructure --startup-project Api
```

4. **Zastosuj migrację**: Środowisko działa na Dockerze, więc po prostu przeładuj kontener, a migracje zostaną automatycznie zastosowane.


## Przykładowe polecenia migracji

| Opis                         | Komenda                                  |
| ---------------------------- | ---------------------------------------- |
| Tworzenie migracji           | `dotnet ef migrations add NazwaMigracji` |
| Usunięcie ostatniej migracji | `dotnet ef migrations remove`            |
| Wykonanie migracji na bazie  | `dotnet ef database update`              |
| Podgląd SQL migracji         | `dotnet ef migrations script`            |
| Sprawdzenie statusu migracji | `dotnet ef database update --verbose`    |
| Lista dostępnych migracji    | `dotnet ef migrations list`              |

# Tworzenie projektu przez CLI

Utwórz projekt

```bash
dotnet new classlib -n Infrastructure -o Infrastructure
```

Dodaj projekt do solucji

```bash
dotnet sln inzynieria_oprogramowania.sln add Infrastructure/Infrastructure.csproj
```

Dodaj referencję do projektu API

```bash
dotnet add Api/Api.csproj reference Infrastructure/Infrastructure.csproj
```

# Dodawanie pakietów NuGet

Entity Framework Core i SQL Server

```bash
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add Infrastructure/Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
```
