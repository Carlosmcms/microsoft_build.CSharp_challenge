# Module 20: Implement logging in a .NET Framework ASP.NET web application

Unless it's specified, apply these commands in Azure Cloud Shell. The project takes *../MigrationWorkshop/eShopLegacyWebFormsSolution* solution, so [check it the example module](https://github.com/Carlosmcms/eShopModernizing.git)
## Basic Configuration

1. Define credentials
```
$useralias = "<user-name>"
$serveradminpassword = "<strong-password>"
$resourcegroupname = "learn-9a232764-3554-4fa6-a39d-ad37fe4ab635"
```

##### *A strong password will be required

2. Define additional variables
```
$location = "eastus"
$webappplanname = (-join($useralias,"-webappplan"))
$webappname = (-join($useralias,"-webapp"))
$serveradminname = "ServerAdmin"
$servername = (-join($useralias, "-workshop-server"))
$dbname = "eShop"
```

3. Create new Azure Aapp Service plan for hosting a web app
```
New-AzAppServicePlan `
    -Name $webappplanname `
    -ResourceGroup $resourcegroupname `
    -Location $location
```

4. Create a web app by using the App Service plan
```
New-AzWebApp `
    -Name $webappname `
    -AppServicePlan $webappplanname `
    -ResourceGroup $resourcegroupname `
    -Location $location
```

5. Assign a managed identity to the web app
```
Set-AzWebApp `
    -AssignIdentity $true `
    -Name $webappname `
    -ResourceGroupName $resourcegroupname
```

6. Create a new Azure SQL Database instance
```
New-AzSqlServer `
    -ServerName $servername `
    -ResourceGroupName $resourcegroupname `
    -Location $location `
    -SqlAdministratorCredentials $(New-Object `
        -TypeName System.Management.Automation.PSCredential `
        -ArgumentList $serveradminname, `
        $(ConvertTo-SecureString `
        -String $serveradminpassword `
        -AsPlainText -Force))
```

7. Open the SQL Database instance firewall to allow access to services hosted in Azure
```
New-AzSqlServerFirewallRule `
    -ResourceGroupName $resourcegroupname `
    -ServerName $servername `
    -FirewallRuleName "AllowedIPs" `
    -StartIpAddress "0.0.0.0" `
    -EndIpAddress "0.0.0.0"
```

8. Create a database in SQL Database. The db will be populated later, when the web app is migrated
```
New-AzSqlDatabase  `
    -ResourceGroupName $resourcegroupname `
    -ServerName $servername `
    -DatabaseName $dbName `
    -RequestedServiceObjectiveName "S0"
```

## Create an Azure Blob Storage account to hold log data

9. Define PowerShell varaibles to contain the names of the storage account and the blob container to create
```
$storageaccountname = (-join($useralias, "storage"))
$storagecontainername = "workshopcontainer"
```

10. Create an Azure Blob Storage account
```
$storageaccount = New-AzStorageAccount `
    -ResourceGroupName $resourcegroupname `
    -Location $location `
    -AccountName $storageaccountname `
    -SkuName Standard_LRS
```

11. Create a container in the storage account to hold the log data
```
New-AzStorageContainer `
    -Name $storagecontainername `
    -Permission Blob `
    -Context $storageaccount.Context
```

12. Generate the connection string for this storage account
```
$storageaccountkey = (Get-AzStorageAccountKey `
    -ResourceGroupName $resourcegroupname `
    -Name $storageaccountname)[0].Value

$storageconnectionstring = `
    ((-join('DefaultEndpointsProtocol=https;AccountName=', `
    $storageaccountname, ';AccountKey=', $storageaccountkey,`
    ';EndpointSuffix=core.windows.net' )))
```

13. Define a PowerShell variable to contains the name of the Application Insights instance to create
```
$appinsightsname = (-join($useralias, "-insights"))
```

14. Create an Application Insights instance
```
$appinsights = New-AzResource `
    -ResourceName $appinsightsname `
    -ResourceGroupName $resourcegroupname `
    -Location $location `
    -ResourceType "Microsoft.Insights/components" `
    -Properties (-join('{"ApplicationId":"', $appinsightsname, '", "Application_Type":"other"}'))
```

15. Retrive the Application Insights instrrumentation key
```
$appinsightskey = $appinsights.Properties.InstrumentationKey
```

