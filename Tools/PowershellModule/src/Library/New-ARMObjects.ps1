# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-ARMObjects
{
    <#
    .SYNOPSIS
    Creates the Azure Resources to stand-up a Windows Package Manager REST Source.

    .DESCRIPTION
    Uses the custom PowerShell object provided by the "New-ARMParameterObject" cmdlet to create Azure resources, and will 
    create the the Key Vault secrets and publish the Windows Package Manager REST source REST apis to the Azure Function.
        
    The following Azure Modules are used by this script:
        Az.Resources
        Az.Accounts
        Az.Websites
        Az.Functions

    .PARAMETER ARMObjects
    Object returned from the "New-ARMParameterObject" providing the paths to the ARM Parameters and Template files.

    .PARAMETER RestSourcePath
    Path to the compiled Function ZIP containing the REST APIs

    .PARAMETER AzResourceGroup
    Resource Group that will be used to create the ARM Objects in.

    .EXAMPLE
    New-ARMObjects -ARMObjects $ARMObjects -RestSourcePath "C:\WinGet-CLI-RestSource\WinGet.RestSource.Functions.zip" -AzResourceGroup "WinGet"

    Parses through the $ARMObjects variable, creating all identified Azure Resources following the provided ARM Parameters and Template information.
    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] $ARMObjects,
        [Parameter(Position=1, Mandatory=$true)] [string] $RestSourcePath,
        [Parameter(Position=2, Mandatory=$true)] [string] $AzResourceGroup
    )
    BEGIN
    {
        ## Imports the contents of the Parameter Files for reference and logging purposes:
        $jsonStorageAccount = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "StorageAccount" }).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsoncdba           = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "CosmosDBAccount"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsonKeyVault       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Keyvault"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json

        ## Azure resource names retrieved from the Parameter files.
        $AzKeyVaultName       = $jsonKeyVault.parameters.name.value
        $AzStorageAccountName = $jsonStorageAccount.parameters.storageAccountName.value
        $CosmosAccountName    = $jsoncdba.Parameters.Name.Value

        ## Azure Keyvault Secret Names - Do not change values (Must match with values in the Template files)
        $AzStorageAccountKeyName      = "AzStorageAccountKey"
        $CosmosAccountEndpointKeyName = "CosmosAccountEndpoint"
        $CosmosAccountKeyWriteKeyName = "CosmosReadWriteKey"
        $CosmosAccountKeyReadKeyName  = "CosmosReadOnlyKey"

        ## Azure Storage Account Connection String Endpoint Suffix
        $AzEndpointSuffix     = "core.windows.net"
    }
    PROCESS
    {
        ## Creates the Azure Resources following the ARM template / parameters
        Write-Information -MessageData "Creating Azure Resources following ARM Templates."
        
        ## This is order specific, please ensure you used the New-ARMParameterObject function to create this object in the pre-determined order.
        foreach ($Object in $ARMObjects) {
            Write-Information -MessageData "  Creating the Azure Object - $($Object.ObjectType)"
    
            ## If the object to be created is an Azure Function, then complete these pre-required steps before creating the Azure Function.
            if($Object.ObjectType -eq "Function") {
                Write-Information -MessageData "    Creating KeyVault Secrets:"

                ## Creates a reference to the Azure Storage Account Connection String as a Secret in the Azure Keyvault.
                $AzStorageAccountKey  = $(Get-AzStorageAccountKey -ResourceGroupName $AzResourceGroup -Name $AzStorageAccountName)[0].Value

                ## Retrieves the required information from the previously created Azure objects. Values will be used to generate required information for the Azure Keyvault.
                $CosmosAccountEndpointValue       = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccount -ResourceGroupName $AzResourceGroup).DocumentEndpoint) -AsPlainText -Force
                $CosmosAccountKeyWriteValue       = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccountKey -ResourceGroupName $AzResourceGroup -Name $CosmosAccountName).PrimaryMasterKey) -AsPlainText -Force
                $CosmosAccountKeyReadValue        = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccountKey -ResourceGroupName $AzResourceGroup -Name $CosmosAccountName).PrimaryReadonlyMasterKey) -AsPlainText -Force
                $AzStorageAccountConnectionString = ConvertTo-SecureString -String "DefaultEndpointsProtocol=https;AccountName=$AzStorageAccountName;AccountKey=$AzStorageAccountKey;EndpointSuffix=$AzEndpointSuffix" -AsPlainText -Force

                ## Adds the Azure Storage Account Connection String to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure Storage Account Connection String."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $AzStorageAccountKeyName -SecretValue $AzStorageAccountConnectionString
    
                ## Adds the Azure Cosmos Account Endpoint URL to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure CosmosDB Connection String."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountEndpointKeyName -SecretValue $CosmosAccountEndpointValue
    
                ## Adds the Azure Cosmos Primary Account Key to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure CosmosDB Write Primary Key."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountKeyWriteKeyName -SecretValue $CosmosAccountKeyWriteValue
   
                ## Adds the Azure Cosmos Primary Account Key to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure CosmosDB Read Primary Key."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountKeyReadKeyName -SecretValue $CosmosAccountKeyReadValue

                ## Create base object of the Azure Function, generating reference object ID for Keyvault
                Write-Information -MessageData "    Creating base Azure Function object."
                $Result = New-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorAction SilentlyContinue
            }
    
            ## Creates the Azure Resource
            Write-Information -MessageData "    Creating $($Object.ObjectType) following the ARM Parameter File..."
            $Result = New-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorAction SilentlyContinue -ErrorVariable objerror -AsJob
    
            while ($Result.State -eq "Running") {
                ## Sets a sleep of 10 seconds after object creation to allow Azure to update creation status, and mark as "running"
                Start-Sleep -Seconds 10
            }
    
            ## Verifies that no error occured when creating the Azure resource
            if($objerror -or $Result.Error) {
                $ErrReturnObject = @{
                    ObjectError = $objerror
                    JobError    = $Result.Error
                }
                ## Creating the object following the ARM template failed.
                Write-Error "Failed to create Azure object." -TargetObject $ErrReturnObject
                Return
            }
            else {
                ## Creating the object was successful
                Write-Information -MessageData "      $($Object.ObjectType) was successfully created."
            }
    
            ## Publish GitHub Functions to newly created Azure Function
            if($Object.ObjectType -eq "Function") {
                ## Gets the Azure Function Name from the Parameter JSON file contents.
                $AzFunctionName = $jsonFunction.parameters.functionName.value
    
                ## Verifies the presence of the "WinGet.RestSource.Functions.zip" file.
                Write-Verbose -Message "    Confirming Compiled Azure Functions is present"
                if(Test-Path $RestSourcePath) {
                    ## The "WinGet.RestSource.Functions.zip" was found in the working directory
                    Write-Verbose -Message "      File Path Found: $RestSourcePath"

                    ## Uploads the Windows Package Manager functions to the Azure Function.
                    Write-Information -MessageData "    Copying function files to the Azure Function."
                    $Result = Publish-AzWebApp -ArchivePath $RestSourcePath -ResourceGroupName $AzResourceGroup -Name $AzFunctionName -Force
                }
                else {
                    $ErrReturnObject = @{
                        FunctionArchivePath = $RestSourcePath
                        TestPathResults     = Test-Path $RestSourcePath
                    }
                    ## The "WinGet.RestSource.Functions.zip" was not found. Unable to uploaded to the Azure Function.
                    Write-Error "File Path not found: $RestSourcePath" -TargetObject $ErrReturnObject
                }
            }
        }
    }
    END
    {
        Return
    }
}