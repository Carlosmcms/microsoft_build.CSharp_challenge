# Module 21: Improve session scalability in a .NET Framework ASP.NET web application by using Azure Cache for Redis

Unless it's specified, apply these commands in Azure Cloud Shell.
## Basic Configuration

1. Define credentials
```
$useralias = "<your-initials-with-suffix>"
$serveradminpassword = "<your-password>"
$resourcegroupname = "[sandbox resource group name]"
```

2. Define variables to create resources
```
$location = "eastus"
$webappplanname = (-join($useralias,"-webappplan"))
$webappname = (-join($useralias,"-webapp"))
$serveradminname = "ServerAdmin"
$servername = (-join($useralias, "-workshop-server"))
$dbname = "eShop"
```

3. Create a new Azure App Service plan for hosting the web app
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

6. Create a new Azure SQL Database
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

7. Open the SQL DB server firewall to allow access to services hosted in Azure
```
New-AzSqlServerFirewallRule `
    -ResourceGroupName $resourcegroupname `
    -ServerName $servername `
    -FirewallRuleName "AllowedIPs" `
    -StartIpAddress "0.0.0.0" `
    -EndIpAddress "0.0.0.0"
```

8. Create database on the SQL DB server
```
New-AzSqlDatabase  `
    -ResourceGroupName $resourcegroupname `
    -ServerName $servername `
    -DatabaseName $dbName `
    -RequestedServiceObjectiveName "S0"
```
## Store session state information in a ASP.NET web app

9. Define a PowerShell variable to contain the name of the Azure Cache for Redis instance
```
$rediscachename = (-join($useralias, "-workshop-cache"))
```

10. Create an Azure Cache for Redis instance
```
az redis create `
    --location $location `
    --name $rediscachename `
    --resource-group $resourcegroupname `
    --sku Basic `
    --vm-size c0
```

11. Check the provisioning state of the cache. Repeat this command every 30 seconds
```
(Get-AzRedisCache `
    -ResourceGroupName $resourcegroupname `
    -Name $rediscachename).ProvisioningState
```

12. Retrieve the primary access key for the cache and record it to use later
```
$rediskey = (Get-AzRedisCacheKey `
    -Name $rediscachename `
    -ResourceGroup $resourcegroupname).PrimaryKey
```