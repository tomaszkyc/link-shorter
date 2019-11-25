# Development info


## Baza danych

### Migracja bazy danych

```
dotnet ef database update
```

### Dodanie odpowiedniej wersji codegenerator'a

```bash
dotnet tool install --global dotnet-aspnet-codegenerator --version 2.2.0
# jeśli powysze krzyknie komunikatem 'Tool 'dotnet-aspnet-codegenerator' is already installed.'
# uruchom polecenia poniej

# usuwa obecny generator
dotnet tool uninstall --global dotnet-aspnet-codegenerator 

# instaluje poprawny generator
dotnet tool install --global dotnet-aspnet-codegenerator --version 2.2.0 
```

### Scaffolding

```bash
dotnet aspnet-codegenerator identity -dc LinkShorter.Models.AppDbContext --files "Account.Register;Account.Login;Account.Logout"
```