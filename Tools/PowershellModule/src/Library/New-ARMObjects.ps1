# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-ARMObjects
{
    <#
    .SYNOPSIS
    Creates the Azure Resources to stand-up a Windows Package Manager REST Source.

    .DESCRIPTION
    Uses the custom PowerShell object provided by the "New-ARMParameterObjects" cmdlet to create Azure resources, and will 
    create the the Key Vault secrets and publish the Windows Package Manager REST source REST apis to the Azure Function.

    .PARAMETER ARMObjects
    Object returned from the "New-ARMParameterObjects" providing the paths to the ARM Parameters and Template files.

    .PARAMETER RestSourcePath
    Path to the compiled Function ZIP containing the REST APIs

    .PARAMETER ResourceGroup
    Resource Group that will be used to create the ARM Objects in.

    .EXAMPLE
    New-ARMObjects -ARMObjects $ARMObjects -RestSourcePath "C:\WinGet-CLI-RestSource\WinGet.RestSource.Functions.zip" -ResourceGroup "WinGet"

    Parses through the $ARMObjects variable, creating all identified Azure Resources following the provided ARM Parameters and Template information.
    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [array]  $ARMObjects,
        [Parameter(Position=1, Mandatory=$true)] [string] $RestSourcePath,
        [Parameter(Position=2, Mandatory=$true)] [string] $ResourceGroup
    )

    # Function to Create Random Strings
    function Create-AppKey()
    {
        $private:characters = 'abcdefghiklmnoprstuvwxyzABCDEFGHIJKLMENOPTSTUVWXYZ'
        $private:randomChars = 1..64 | ForEach-Object { Get-Random -Maximum $characters.length }
    
        # Set the output field separator to empty instead of space
        $private:ofs=""
        return [String]$characters[$randomChars]
    }

    ## Azure resource names retrieved from the Parameter files.
    $StorageAccountName   = $ARMObjects.Where({$_.ObjectType -eq "StorageAccount"}).Parameters.Parameters.storageAccountName.value
    $KeyVaultName         = $ARMObjects.Where({$_.ObjectType -eq "Keyvault"}).Parameters.Parameters.name.value
    $CosmosAccountName    = $ARMObjects.Where({$_.ObjectType -eq "CosmosDBAccount"}).Parameters.Parameters.name.value
    $AppConfigName        = $ARMObjects.Where({$_.ObjectType -eq "AppConfig"}).Parameters.Parameters.appConfigName.value

    ## Azure Keyvault Secret Names - Do not change values (Must match with values in the Template files)
    $CosmosAccountEndpointKeyName = "CosmosAccountEndpoint"
    $AzureFunctionHostKeyName = "AzureFunctionHostKey"
    $AppConfigPrimaryEndpointName = "AppConfigPrimaryEndpoint"
    $AppConfigSecondaryEndpointName = "AppConfigSecondaryEndpoint"
    
    $FunctionAppUrls = @()
    
    ## Creates the Azure Resources following the ARM template / parameters
    Write-Information "Creating Azure Resources following ARM Templates."
    
    ## This is order specific, please ensure you used the New-ARMParameterObjects function to create this object in the pre-determined order.
    foreach ($Object in $ARMObjects) {
        Write-Information "  Creating the Azure Object - $($Object.ObjectType)"
    
        ## If the object to be created is an Azure Function, then complete these pre-required steps before creating the Azure Function.
        if ($Object.ObjectType -eq "Function") {
            Write-Verbose "    Creating KeyVault Secrets:"

            $CosmosAccountEndpointValue = $(Get-AzCosmosDBAccount -Name $CosmosAccountName -ResourceGroupName $ResourceGroup).DocumentEndpoint | ConvertTo-SecureString -AsPlainText -Force
            Write-Verbose "      Creating Keyvault Secret for Azure CosmosDB endpoint."
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $CosmosAccountEndpointKeyName -SecretValue $CosmosAccountEndpointValue

            $AppConfigEndpointValue = $(Get-AzAppConfigurationStore -Name azpstest-appstore -ResourceGroupName $ResourceGroup).Endpoint | ConvertTo-SecureString -AsPlainText -Force
            Write-Verbose "      Creating Keyvault Secret for Azure App Config endpoint."
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $AppConfigPrimaryEndpointName -SecretValue $AppConfigEndpointValue
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $AppConfigSecondaryEndpointName -SecretValue $AppConfigEndpointValue
        }
        elseif ($Object.ObjectType -eq "ApiManagement") {
            ## Create instance manually if not exist
            $ApiManagementParameters = $Object.Parameters.Parameters
            $ApiManagement = Get-AzApiManagement -ResourceGroupName $ResourceGroup -Name $ApiManagementParameters.serviceName.value
            if (!$ApiManagement) {
                Write-Information "Creating new Api Aanagement service. Name: $($ApiManagementParameters.serviceName.value)"
                $ApiManagement = New-AzApiManagement -ResourceGroupName $ResourceGroup -Name $ApiManagementParameters.serviceName.value -Location $ApiManagementParameters.location.value -Organization $ApiManagementParameters.publisherName.value -AdminEmail $ApiManagementParameters.publisherEmail.value -Sku $ApiManagementParameters.sku.value -SystemAssignedIdentity -ErrorVariable DeployError
                if ($DeployError) {
                    Write-Error "Failed to create Api Aanagement service. $DeployError"
                    return $false
                }
            }

            ## Update backend urls and re-create parameters file
            $ApiManagementParameters.backendUrls.value = $FunctionAppUrls
            Write-Verbose -Message "  Re-creating the Parameter file for $($Object.ObjectType) in the following location: $($Object.ParameterPath)"
            $ParameterFile = $Object.Parameters | ConvertTo-Json -Depth 8
            $ParameterFile | Out-File -FilePath $Object.ParameterPath -Force

            ## Set secret get for Api management service
            Set-AzKeyVaultAccessPolicy -VaultName $KeyVaultName -ResourceGroupName $ResourceGroup -ObjectId $ApiManagement.Identity.PrincipalId -PermissionsToSecrets Get
        }
    
        ## Creates the Azure Resource
        $DeployJob = New-AzResourceGroupDeployment -ResourceGroupName $ResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorVariable DeployError -AsJob
    
        while ($DeployJob.State -eq "Running") {
            ## Sets a sleep of 2 seconds after object creation to allow Azure to update creation status, and mark as "running"
            Start-Sleep -Seconds 2
        }

        ## Verifies that no error occured when creating the Azure resource
        if ($DeployError -or $DeployJob.State -ne "Completed") {
            $ErrReturnObject = @{
                DeployError = $DeployError
                JobError    = $DeployJob
            }

            ## TODO: extend error reporting in logs across scripts.
            Write-Error "Failed to create Azure object. $DeployError" -TargetObject $ErrReturnObject
            return $false
        }

        Write-Information -MessageData "      $($Object.ObjectType) was successfully created."

        ## Publish GitHub Functions to newly created Azure Function
        if($Object.ObjectType -eq "Function") {
            $FunctionName = $Object.Parameters.Parameters.functionName.value
            $FunctionApp = Get-AzFunctionApp -ResourceGroupName $ResourceGroup -Name $FunctionName
            $FunctionAppId = $FunctionApp.Id

            $FunctionAppUrls += "https://$($FunctionApp.DefaultHostName)/api"
            
            ## Create Function app key and also add to keyvault
            $local:NewFunctionKeyValue = Create-AppKey
            $Result = Invoke-AzRestMethod -Path "$FunctionAppId/host/default/functionKeys/WinGetRestSourceAccess?api-version=2024-04-01" -Method PUT -Payload (@{properties=@{value = $NewFunctionKeyValue}} | ConvertTo-Json -Depth 8)
            if ($Result.StatusCode -ne 200 -and $Result.StatusCode -ne 201) {
                Write-Error "Failed to create Azure Function key. $($Result.Content)"
                return $false
            }
            
            Write-Information -MessageData "    Add Function App host key to keyvault."
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $AzureFunctionHostKeyName -SecretValue ($NewFunctionKeyValue | ConvertTo-SecureString -AsPlainText -Force)

            ## Assign necessary roles
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "Storage Account Contributor" -ResourceGroupName $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "Storage Blob Data Owner" -ResourceGroupName $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "Storage Table Data Contributor" -ResourceGroupName $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "Storage Queue Data Contributor" -ResourceGroupName $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "Storage Queue Data Message Processor" -ResourceGroupName $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "Storage Queue Data Message Sender" -ResourceGroupName $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            New-AzRoleAssignment -ObjectId $FunctionApp.Identity.PrincipalId -RoleDefinitionName "App Configuration Data Reader" -ResourceGroupName $ResourceGroup -ResourceName $AppConfigName -ResourceType "Microsoft.AppConfiguration/configurationStores"

            ## Assign cosmos db roles
            $RoleId = [guid]::NewGuid().ToString()
            $CosmosDBDataAction = @("Microsoft.DocumentDB/databaseAccounts/readMetadata", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*", "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*")
            New-AzCosmosDBSqlRoleDefinition -AccountName $CosmosAccountName -ResourceGroupName $ResourceGroup -Type CustomRole -RoleName "ReadWriteAll" -AssignableScope "/" -DataAction $CosmosDBDataAction -Id $RoleId
            New-AzCosmosDBSqlRoleAssignment -AccountName $CosmosAccountName -ResourceGroupName $ResourceGroup -RoleDefinitionId $RoleId -Scope "/" -PrincipalId $FunctionApp.Identity.PrincipalId

            Write-Information -MessageData "    Publishing function files to the Azure Function."
            $DeployJob = Publish-AzWebApp -ArchivePath $RestSourcePath -ResourceGroupName $ResourceGroup -Name $FunctionName -Force -AsJob -ErrorVariable DeployError
                while ($DeployJob.State -eq "Running") {
                Start-Sleep -Seconds 2
            }
            
            ## Verifies that no error occured when publishing the Function App
            if ($DeployError -or $DeployJob.State -ne "Completed") {
                $ErrReturnObject = @{
                    DeployError = $DeployError
                    JobError    = $DeployJob
                }

                Write-Error "Failed to publishing the Function App. $DeployError" -TargetObject $ErrReturnObject
                return $false
            }
            
            ## Restart the Function App
            Restart-AzFunctionApp -Name $FunctionName -ResourceGroupName $ResourceGroup -Force
        }
    }
    
    return $true
}
