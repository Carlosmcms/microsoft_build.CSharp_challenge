# Razor

## Run Razor Project while update on saving code
dotnet watch

## Create new Razor page
dotnet new page --name PizzaList --namespace ContosoPizza.Pages --output Pages

# .NET

## Create a new API project
```dotnet new webapi -f net7.0```

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

