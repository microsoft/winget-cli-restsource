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

    # Function to create a new Function App key
    function New-FunctionAppKey
    {
        $private:characters = 'abcdefghiklmnoprstuvwxyzABCDEFGHIJKLMENOPTSTUVWXYZ'
        $private:randomChars = 1..64 | ForEach-Object { Get-Random -Maximum $characters.length }
    
        # Set the output field separator to empty instead of space
        $private:ofs=""
        return [String]$characters[$randomChars]
    }

    # Function to ensure Azure role assignment
    function Set-RoleAssignment
    {
        param(
            [string] $PrincipalId,
            [string] $RoleName,
            [string] $ResourceGroup,
            [string] $ResourceName,
            [string] $ResourceType
        )
        
        $GetAssignment = Get-AzRoleAssignment -ObjectId $PrincipalId -RoleDefinitionName $RoleName -ResourceGroupName $ResourceGroup -ResourceName $ResourceName -ResourceType $ResourceType
        if (!$GetAssignment) {
            Write-Verbose "Creating role assignment. Role: $RoleName"
            New-AzRoleAssignment -ObjectId $PrincipalId -RoleDefinitionName $RoleName -ResourceGroupName $ResourceGroup -ResourceName $ResourceName -ResourceType $ResourceType
        }
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
        Write-Information "Creating the Azure Object - $($Object.ObjectType)"

        ## If the object to be created is an Azure Function, then complete these pre-required steps before creating the Azure Function.
        if ($Object.ObjectType -eq "Function") {
            $CosmosAccountEndpointValue = $(Get-AzCosmosDBAccount -Name $CosmosAccountName -ResourceGroupName $ResourceGroup).DocumentEndpoint | ConvertTo-SecureString -AsPlainText -Force
            Write-Verbose "Creating Keyvault Secret for Azure CosmosDB endpoint."
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $CosmosAccountEndpointKeyName -SecretValue $CosmosAccountEndpointValue

            $AppConfigEndpointValue = $(Get-AzAppConfigurationStore -Name $AppConfigName -ResourceGroupName $ResourceGroup).Endpoint | ConvertTo-SecureString -AsPlainText -Force
            Write-Verbose "Creating Keyvault Secret for Azure App Config endpoint."
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $AppConfigPrimaryEndpointName -SecretValue $AppConfigEndpointValue
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $AppConfigSecondaryEndpointName -SecretValue $AppConfigEndpointValue
        }
        elseif ($Object.ObjectType -eq "ApiManagement") {
            ## Create instance manually if not exist
            $ApiManagementParameters = $Object.Parameters.Parameters
            $ApiManagement = Get-AzApiManagement -ResourceGroupName $ResourceGroup -Name $ApiManagementParameters.serviceName.value -ErrorVariable ErrorGet -ErrorAction SilentlyContinue
            if (!$ApiManagement) {
                Write-Warning "Creating new Api Aanagement service. Name: $($ApiManagementParameters.serviceName.value)"
                Write-Warning "This is a long-running action. It can take between 30 and 40 minutes to create and activate an API Management service."
                $ApiManagement = New-AzApiManagement -ResourceGroupName $ResourceGroup -Name $ApiManagementParameters.serviceName.value -Location $ApiManagementParameters.location.value -Organization $ApiManagementParameters.publisherName.value -AdminEmail $ApiManagementParameters.publisherEmail.value -Sku $ApiManagementParameters.sku.value -SystemAssignedIdentity -ErrorVariable DeployError
                if ($DeployError) {
                    Write-Error "Failed to create Api Aanagement service. Error: $DeployError"
                    return $false
                }
            }

            ## Update backend urls and re-create parameters file if needed
            if (!$ApiManagementParameters.backendUrls.value) {
                $ApiManagementParameters.backendUrls.value = $FunctionAppUrls
                Write-Verbose -Message "Re-creating the Parameter file for $($Object.ObjectType) in the following location: $($Object.ParameterPath)"
                $ParameterFile = $Object.Parameters | ConvertTo-Json -Depth 8
                $ParameterFile | Out-File -FilePath $Object.ParameterPath -Force
            }

            ## Set secret get for Api management service
            Write-Verbose "Set keyvault secret access for Api Management service"
            Set-AzKeyVaultAccessPolicy -VaultName $KeyVaultName -ResourceGroupName $ResourceGroup -ObjectId $ApiManagement.Identity.PrincipalId -PermissionsToSecrets Get
        }

        ## Creates the Azure Resource
        $DeployResult = New-AzResourceGroupDeployment -ResourceGroupName $ResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorVariable DeployError

        ## Verifies that no error occured when creating the Azure resource
        if ($DeployError -or ($DeployResult.ProvisioningState -ne "Succeeded" -and $DeployResult.ProvisioningState -ne "Created")) {
            $ErrReturnObject = @{
                DeployError = $DeployError
                DeployResult = $DeployResult
            }

            Write-Error "Failed to create Azure object. Error: $DeployError" -TargetObject $ErrReturnObject
            return $false
        }

        Write-Information -MessageData "$($Object.ObjectType) was successfully created."

        if($Object.ObjectType -eq "Function") {
            ## Publish GitHub Functions to newly created Azure Function
            $FunctionName = $Object.Parameters.Parameters.functionName.value
            $FunctionApp = Get-AzFunctionApp -ResourceGroupName $ResourceGroup -Name $FunctionName
            $FunctionAppId = $FunctionApp.Id

            $FunctionAppUrls += "https://$($FunctionApp.DefaultHostName)/api"

            ## Assign necessary Azure roles
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "Storage Account Contributor" -ResourceGroup $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "Storage Blob Data Owner" -ResourceGroup $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "Storage Table Data Contributor" -ResourceGroup $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "Storage Queue Data Contributor" -ResourceGroup $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "Storage Queue Data Message Processor" -ResourceGroup $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "Storage Queue Data Message Sender" -ResourceGroup $ResourceGroup -ResourceName $StorageAccountName -ResourceType "Microsoft.Storage/storageAccounts"
            Set-RoleAssignment -PrincipalId $FunctionApp.IdentityPrincipalId -RoleName "App Configuration Data Reader" -ResourceGroup $ResourceGroup -ResourceName $AppConfigName -ResourceType "Microsoft.AppConfiguration/configurationStores"

            ## Set keyvault secrets get permission
            Write-Verbose "Set keyvault secret access for Azure Function"
            Set-AzKeyVaultAccessPolicy -VaultName $KeyVaultName -ResourceGroupName $ResourceGroup -ObjectId $FunctionApp.IdentityPrincipalId -PermissionsToSecrets Get

            ## Assign cosmos db roles
            $CosmosAccount = Get-AzCosmosDBAccount -ResourceGroupName $ResourceGroup -Name $CosmosAccountName
            $RoleId = "00000000-0000-0000-0000-000000000002" # Built in contributor role
            $RoleDefinitionId = "$($CosmosAccount.Id)/sqlRoleAssignments/$RoleId"
            if ((Get-AzCosmosDBSqlRoleAssignment -ResourceGroupName $ResourceGroup -AccountName $CosmosAccountName).Where({$_.PrincipalId -eq $FunctionApp.IdentityPrincipalId -and $_.RoleDefinitionId -eq $RoleDefinitionId}).Count -eq 0) {
                Write-Verbose "Assigning Cosmos DB Account contributor role"
                New-AzCosmosDBSqlRoleAssignment -AccountName $CosmosAccountName -ResourceGroupName $ResourceGroup -RoleDefinitionId $RoleId -Scope "/" -PrincipalId $FunctionApp.IdentityPrincipalId
            }

            ## Create Function app key and also add to keyvault
            $NewFunctionKeyValue = New-FunctionAppKey
            $Result = Invoke-AzRestMethod -Path "$FunctionAppId/host/default/functionKeys/WinGetRestSourceAccess?api-version=2024-04-01" -Method PUT -Payload (@{properties=@{value = $NewFunctionKeyValue}} | ConvertTo-Json -Depth 8)
            if ($Result.StatusCode -ne 200 -and $Result.StatusCode -ne 201) {
                Write-Error "Failed to create Azure Function key. $($Result.Content)"
                return $false
            }

            Write-Information -MessageData "Add Function App host key to keyvault."
            Set-AzKeyVaultSecret -VaultName $KeyVaultName -Name $AzureFunctionHostKeyName -SecretValue ($NewFunctionKeyValue | ConvertTo-SecureString -AsPlainText -Force)

            Write-Information -MessageData "Publishing function files to the Azure Function."
            $DeployResult = Publish-AzWebApp -ArchivePath $RestSourcePath -ResourceGroupName $ResourceGroup -Name $FunctionName -Force -ErrorVariable DeployError

            ## Verifies that no error occured when publishing the Function App
            if ($DeployError -or !$DeployResult) {
                $ErrReturnObject = @{
                    DeployError = $DeployError
                    DeployResult = $DeployResult
                }

                Write-Error "Failed to publishing the Function App. $DeployError" -TargetObject $ErrReturnObject
                return $false
            }

            ## Restart the Function App
            Restart-AzFunctionApp -Name $FunctionName -ResourceGroupName $ResourceGroup -Force
        }
        elseif ($Object.ObjectType -eq "AppConfig") {
            ## Update App Config values if needed
            $AppConfigParameters = $Object.Parameters.Parameters
            if (!$AppConfigParameters.deployAppConfigValues.value) {
                ## This is local deployment
                Write-Information "Updating App Config values"

                ## Assign Data Owner role
                $AzContext = Get-AzContext
                if ($AzContext.Account.Type -eq "User") {
                    $AzObjectID = $(Get-AzADUser -SignedIn).Id
                }
                else {
                    $AzObjectID = $(Get-AzADServicePrincipal -ApplicationId $AzContext.Account.ID).Id
                }
                Set-RoleAssignment -PrincipalId $AzObjectID -RoleName "App Configuration Data Owner" -ResourceGroup $ResourceGroup -ResourceName $AppConfigParameters.appConfigName.value -ResourceType "Microsoft.AppConfiguration/configurationStores"

                $AppConfig = Get-AzAppConfigurationStore -ResourceGroupName $ResourceGroup -Name $AppConfigParameters.appConfigName.value
                $Path = "$($AppConfig.Id)?api-version=2022-05-01"
                $Body = @{ properties = @{ disableLocalAuth = $false } } | ConvertTo-Json -Depth 8
                Invoke-AzRestMethod -Method Patch -Path $Path -Payload $Body

                ## Updating config values. Currently only feature flags.
                $FeatureFlagValue = @{
                    id = "GenevaLogging"
                    description = "Feature flag to use Geneva Monitoring."
                    enabled = $false
                    conditions = @{ client_filters = @() } } | ConvertTo-Json -Depth 8
                Set-AzAppConfigurationKeyValue -Endpoint $AppConfig.Endpoint -Key ".appconfig.featureflag/GenevaLogging" -Value $FeatureFlagValue

                ## Re-enable disable local auth
                Update-AzAppConfigurationStore -ResourceGroupName $ResourceGroup -Name $AppConfigParameters.appConfigName.value -DisableLocalAuth
            }
        }
    }

    return $true
}
