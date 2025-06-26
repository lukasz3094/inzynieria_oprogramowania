# Start apki:

Z katalogu głównego projektu uruchom polecenie:

```bash
docker-compose down --volumes
docker-compose up --build
```

## Następnie otwórz przeglądarkę i przejdź do `http://localhost:5001/swagger/index.html`, aby zobaczyć dokumentację API.


# Połączenie frontendu z backendem
Aby połączyć frontend z backendem, upewnij się, że w pliku konfiguracyjnym frontendu (np. `src/config.js` lub `src/constants.js`) masz ustawiony adres URL API:

```javascript
export const API_URL = 'http://localhost:5001/api';
```


# Migracje

Migracje bazy danych są automatycznie tworzone i stosowane podczas uruchamiania aplikacji. Jeśli chcesz ręcznie utworzyć migrację, możesz użyć następującego polecenia:

```bash
dotnet ef migrations add <NazwaMigracji>
```

Aby zastosować migracje, użyj:

```bash
dotnet ef database update
```

## Dodawanie modelu do aplikacji/tabeli do bazy danych
Aby dodać nowy model do aplikacji i utworzyć odpowiadającą mu tabelę w bazie danych, wykonaj następujące kroki:

1. **Dodaj nowy model**: Utwórz nową klasę w katalogu `Api/Data`, która będzie reprezentować tabelę w bazie danych. Na przykład:

```csharp
public class NowyModel
{
    public int Id { get; set; }
    public string Nazwa { get; set; }
}
```

2. **Zaktualizuj kontekst bazy danych**: Dodaj nowy model do klasy `AppDbContext` w katalogu `Api/Data/AppDbContext.cs`:

```csharp
public DbSet<NowyModel> NoweModele { get; set; }
```

3. **Utwórz migrację**: W terminalu, w katalogu głównym projektu, uruchom polecenie:
```bash
dotnet ef migrations add DodajNowyModel
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
