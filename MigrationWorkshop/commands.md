# Azure credentials

Unless it's specified, apply these commands in Azure Cloud Shell.

## Basic Configuration

1. Define credentials
```
$useralias = "<user-name>"
$serveradminpassword = "<user-name>"
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

## Create an Azure key vault

9. Define a PowerShell variable to contain the name of the key vault
```
$vaultname = (-join("shopvault", $useralias))
```
10. Create the Azure key vault
```
New-AzKeyVault `
    -Name $vaultname `
    -ResourceGroupName $resourcegroupname `
    -location $location
```

11. Retrieve the service principal ID of the web app
```
$appId=(Get-AzWebApp `
    -ResourceGroupName $resourcegroupname `
    -Name $webappname).Identity.PrincipalId
```

12. Set the access policy of the key vault to allow the web app, which will be identified by using the service principal
```
Set-AzKeyVaultAccessPolicy `
    -VaultName $vaultname `
    -ResourceGroupName $resourcegroupname `
    -ObjectId $appId `
    -PermissionsToSecrets Get
```

13. Generate the connection string for the SQL Server DB by using PowerShell variables
```
$connectionstringplaintext = `
    (-join("Server=tcp:", $servername, ".database.windows.net,1433;Database=", `
    $dbname, ";User ID=", $serveradminname, ";Password=", $serveradminpassword, `
    ";Encrypt=true;Connection Timeout=30;"))
```

14. Convert the connection string into a secure string
```
$connectionstring = ConvertTo-SecureString $connectionstringplaintext `
    -AsPlainText `
    -Force
```

15. Find the object ID of the accout in Azure AD
```
$objectId = az ad signed-in-user show `
    --query objectId -o tsv
```

16. Grant the account privileges to create and retrieve secrets and keys from the key vault
```
Set-AzKeyVaultAccessPolicy `
    -VaultName $vaultname `
    -PermissionsToKeys create,decrypt,delete,encrypt,get,list,unwrapKey,wrapKey `
    -PermissionsToSecrets get,list,set,delete `
    -ObjectId $objectId
```

17. Store the secure string in the key vault by using the key CatalogDBContext.
```
Set-AzKeyVaultSecret `
    -VaultName $vaultname `
    -Name "CatalogDBContext" `
    -SecretValue $connectionstring
```

18. Verify that the *CatalogDBContext* secret has been stored in the key vault.
```
Get-AzKeyVaultSecret `
    -VaultName $vaultname `
    -Name "CatalogDBContext"
```

19. Set the vault name as an AppSetting name *KeyVaultName* for the web app.
```
Set-AzWebApp `
    -Name $webappname `
    -ResourceGroupName $resourcegroupname `
    -AppSettings @{KeyVaultName = $vaultname}
```