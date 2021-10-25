Function New-ARMObjects
{
    <#
    .SYNOPSIS
    Creates the Azure Resources to stand-up a Windows Package Manager Rest Source.

    .DESCRIPTION
    Uses the custom PowerShell object provided by the "New-ARMParameterObject" cmdlet to create Azure resources, and will create the the Key Vault secrets and publish the Windows Package Manager private source rest apis to the Azure Function.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp

    .PARAMETER ARMObjects
    Object returned from the "New-ARMParamterObject" providing the paths to the ARM Parameters and Template files.

    .PARAMETER ArchiveFunctionZip
    Path to the compiled Function ZIP containing the Rest APIs

    .PARAMETER AzResourceGroup
    Resource Group that will be used to create the ARM Objects in.

    .EXAMPLE
    New-ARMObjects -ARMObjects $ARMObjects -ArchiveFunctionZip "C:\WinGet-CLI-RestSource\CompiledFunction.zip" -AzResourceGroup "WinGetResourceGroup"

    Parses through the $ARMObjects variable, creating all identified Azure Resources following the provided ARM Parameters and Template information.
    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] $ARMObjects,
        [Parameter(Position=1, Mandatory=$true)] [string] $ArchiveFunctionZip,
        [Parameter(Position=2, Mandatory=$true)] [string] $AzResourceGroup
    )
    BEGIN
    {
        ## Path to the compiled Functions compressed into a Zip file for upload to Azure Function
        #$ArchiveFunctionZip = "$WorkingDirectory\CompiledFunctions.zip"
        
        ## Imports the contents of the Parameter Files for reference and logging purposes:
        $jsonStorageAccount = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "StorageAccount" }).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsoncdba           = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "CosmosDBAccount"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsonKeyVault       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Keyvault"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json
        $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) -ErrorAction SilentlyContinue | ConvertFrom-Json

        ## Azure resource names retrieved from the Parameter files.
        $AzKeyVaultName       = $jsonKeyVault.parameters.name.value
        $AzStorageAccountName = $jsonStorageAccount.parameters.storageAccountName.value
        $CosmosAccountName    = $jsoncdba.Parameters.Name.Value

        ## Azure Keyvault Secret Names - Do not change values
        $AzStorageAccountKeyName      = "AzStorageAccountKey"
        $CosmosAccountEndpointKeyName = "CosmosAccountEndpoint"
        $CosmosAccountKeyKeyName      = "CosmosAccountKey"

        ## Azure Storage Account Connection String Endpoint Suffix
        $AzEndpointSuffix     = "core.windows.net"
    }
    PROCESS
    {
        ## Creates the Azure Resources following the ARM template / parameters
        Write-Information -MessageData "Creating Azure Resources following ARM Templates."
        
        foreach ($Object in $ARMObjects) {
            Write-Information -MessageData "  Creating the Azure Object - $($Object.ObjectType)"
    
            ## If the object to be created is an Azure Function, then complete these pre-required steps before creating the Azure Function.
            if($Object.ObjectType -eq "Function") {
                Write-Information -MessageData "    Creating KeyVault Secrets:"

                ## Creates a reference to the Azure Storage Account Connection String as a Secret in the Azure Keyvault.
                $AzStorageAccountKey  = $(Get-AzStorageAccountKey -ResourceGroupName $AzResourceGroup -Name $AzStorageAccountName)[0].Value

                ## Retrieves the required information from the previously created Azure objects. Values will be used to generate required information for the Azure Keyvault.
                $CosmosAccountEndpointValue       = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccount -ResourceGroupName $AzResourceGroup).DocumentEndpoint) -AsPlainText -Force
                $CosmosAccountKeyValue            = ConvertTo-SecureString -String $($(Get-AzCosmosDBAccountKey -ResourceGroupName $AzResourceGroup -Name $CosmosAccountName).PrimaryMasterKey) -AsPlainText -Force
                $AzStorageAccountConnectionString = ConvertTo-SecureString -String "DefaultEndpointsProtocol=https;AccountName=$AzStorageAccountName;AccountKey=$AzStorageAccountKey;EndpointSuffix=$AzEndpointSuffix" -AsPlainText -Force

                ## Adds the Azure Storage Account Connection String to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure Storage Account Connection String."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $AzStorageAccountKeyName -SecretValue $AzStorageAccountConnectionString
    
                ## Adds the Azure Cosmos Account Endpoint URL to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure CosmosDB Connection String."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountEndpointKeyName -SecretValue $CosmosAccountEndpointValue
    
                ## Adds the Azure Cosmos Primary Account Key to the Keyvault
                Write-Information -MessageData "      Creating Keyvault Secret for Azure CosmosDB Primary Key."
                $Result = Set-AzKeyVaultSecret -VaultName $AzKeyVaultName -Name $CosmosAccountKeyKeyName -SecretValue $CosmosAccountKeyValue
    
                ## Create base object of the Azure Function, generating reference object ID for Keyvault
                Write-Information -MessageData "    Creating base Azure Function object."
                $Result = New-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorAction SilentlyContinue
            }
    
            ## Creates the Azure Resource
            Write-Information -MessageData "    Creating $($Object.ObjectType) following the ARM Parameter File..."
            $Result = New-AzResourceGroupDeployment -ResourceGroupName $AzResourceGroup -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath -Mode Incremental -ErrorAction SilentlyContinue -ErrorVariable objerror
    
            ## Sets a sleep of 10 seconds after object creation to allow Azure to update creation status, and mark as "running"
            Start-Sleep -Seconds 10
    
            ## Verifies that no error occured when creating the Azure resource
            if($objerror) {
                $ErrReturnObject = @{
                    ObjectError = $objerror
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
    
                ## Verifies the presence of the "CompiledFunctions.zip" file.
                Write-Verbose -Message "    Confirming Compiled Azure Functions is present"
                if(Test-Path $ArchiveFunctionZip) {
                    ## The "CompiledFunctions.zip" was found in the working directory
                    Write-Verbose -Message "      File Path Found: $ArchiveFunctionZip"

                    ## Uploads the Windows Package Manager functions to the Azure Function.
                    Write-Information -MessageData "    Copying function files to the Azure Function."
                    $Result = Publish-AzWebApp -ArchivePath $ArchiveFunctionZip -ResourceGroupName $AzResourceGroup -Name $AzFunctionName -Force
                }
                else {
                    $ErrReturnObject = @{
                        FunctionArchivePath = $ArchiveFunctionZip
                        TestPathResults     = Test-Path $ArchiveFunctionZip
                    }
                    ## The "CompiledFunctions.zip" was not found. Unable to uploaded to the Azure Function.
                    Write-Error "File Path not found: $ArchiveFunctionZip" -TargetObject $ErrReturnObject
                }
            }
        }
    }
    END
    {
        Return
    }
}