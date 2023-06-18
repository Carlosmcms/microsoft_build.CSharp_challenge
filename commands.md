# Project Creation
## Create a new .NET console project
```dotnet new console -f net6.0```

## Create a new API project
```dotnet new webapi -f net7.0```

## Create a new Razor app (no https required)
```dotnet new blazorserver --no-https true -f net6.0```

## Create a new Blazor WebAssembly app
```dotnet new blazorwasm -o AppName```

## Create a minimal API project
```dotnet new web -o projectname -f net6.0```

## Create React app
```npx create-react-app pizza-web```

# Razor

## Run Razor Project while update on saving code
```dotnet watch```

## Create new Razor page
```dotnet new page --name PizzaList --namespace ContosoPizza.Pages --output Pages```

## Create a new Razor component
```dotnet new razorcomponent -n ComponentName -o Folder```

## Create a library-class component
```dotnet new razorclasslib -o MyProjectName```

## Add library-class component reference to project
```dotnet add reference ../Path-to-library```

## Package the library for reuse
```dotnet pack```

## Add reference for NuGet package
```dotnet add package My.FirstClassLibrary -s ..\Path-to-NuGet-library.nupkg```

# .NET

## Install a package || version specificity || Include prerelease
```dotnet add package {PACKAGE_NAME} || --version={VERSION_NUMBER} || --prerelease```

## List .NET packages || Include transitive (Globals) || list outdated packages
```dotnet list package || --include-transitive || --outdated```

## Restore dependencies
```dotnet restore```

## Configure system to trust dev certificate
```dotnet dev-certs https --trust```

## Build a .NET project
```dotnet build```

## Run a .NET project
```dotnet run```

# HTTP REPL

## Install .NET REPL
```dotnet tool install -g Microsoft.dotnet-httprepl```

## Start HTTP REPL
```httprepl https://localhost:{PORT}```

### Connect to the URL (while in httprepl)
```connect https://localhost:{PORT}```

### List available endpoints (while in httprepl)
```ls```

### Go to an endpoint (while in httpprepl)
```cd {ENDPOINT}```

### Make a GET request (while in httprepl)
```get```

### Make a POST request (while in httprepl)
```post -c "{"property1":"value1", "propertyN":"valueN"}"```

### Make a PUT request (while in httprepl)
```put {ID} -c "{"id":{ID}, "property1":"value1", "propertyN":"valueN"}"```

### Make a DELETE request (while in httprepl)
```delete {ID}```

### Exit httprepl
```exit```

# Packages commands

## Newtonsoft
### Parsing json files
```dotnet add package Newtonsoft.Json```

## Entity Framework

### Install EF
```dotnet add package Microsoft.EntityFrameworkCore --version 6.0.8```

### SQLite for Entity Framework

```dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0.8```

### Add EF InMemory
```dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 6.0```

### Add EF Core Tools (Create and apply migrations, generate code for a model based on an existing database)
```dotnet tool install --global dotnet-ef```

### Add EF Core.Design (Design-time logic for EF to create the database)
```dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0```

### Create a a db migration
```dotnet ef migrations add migration-name```

### Apply migration
```dotnet ef database update```

### HTTP.json
```dotnet add package System.Net.Http.Json --version 6.0.0```

## Swagger

### Install Swashbuckle
```dotnet add package Swashbuckle.AspNetCore --version 6.1.4```

