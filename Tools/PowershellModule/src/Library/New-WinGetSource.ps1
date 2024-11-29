# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-WinGetSource
{
    <#
    .SYNOPSIS
    Creates a Windows Package Manager REST source in Azure for the storage of Windows Package Manager package Manifests.

    .DESCRIPTION
    Creates a Windows Package Manager REST source in Azure for the storage of Windows Package Manager package Manifests.

    .PARAMETER Name
    The name of the objects that will be created

    .PARAMETER ResourceGroup
    [Optional] The name of the Resource Group that the Windows Package Manager REST source will reside. All Azure
    resources will be created in in this Resource Group (Default: WinGetRestsource)

    .PARAMETER SubscriptionName
    [Optional] The name of the subscription that will be used to host the Windows Package Manager REST source. (Default: Current connected subscription)

    .PARAMETER Region
    [Optional] The Azure location where objects will be created in. (Default: westus)

    .PARAMETER TemplateFolderPath
    [Optional] The directory containing required ARM templates. (Default: $PSScriptRoot\..\Data\ARMTemplate)
    
    .PARAMETER ParameterOutputPath
    [Optional] The directory where Parameter objects will be created in. (Default: Current Directory\Parameters)

    .PARAMETER RestSourcePath
    [Optional] Path to the compiled REST API Zip file. (Default: $PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip)

    .PARAMETER ImplementationPerformance
    [Optional] ["Developer", "Basic", "Enhanced"] specifies the performance of the resources to be created for the Windows Package Manager REST source.
    | Preference | Description                                                                                                             |
    |------------|-------------------------------------------------------------------------------------------------------------------------|
    | Developer  | Specifies lowest cost for developing the Windows Package Manager REST source. Uses free-tier options when available.    |
    | Basic      | Specifies a basic functioning Windows Package Manager REST source.                                                      |
    | Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers.                               |

    (Default: Basic)

    .PARAMETER ShowConnectionInstructions
    [Optional] If specified, the instructions for connecting to the Windows Package Manager REST source. (Default: False)

    .EXAMPLE
    New-WinGetSource -Name "contosorestsource"

    Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of
    Azure with the basic level performance.

    .EXAMPLE
    New-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -SubscriptionName "Visual Studio Subscription" -Region "westus" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic" -ShowConnectionInformation

    Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of
    Azure with the basic level performance in the "Visual Studio Subscription" Subscription. Displays the required command
    to connect the WinGet client to the new REST source after the REST source has been created.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$Name,
        [Parameter(Position=1, Mandatory=$false)] [string]$ResourceGroup = "WinGetRestSource",
        [Parameter(Position=2, Mandatory=$false)] [string]$SubscriptionName = "",
        [Parameter(Position=3, Mandatory=$false)] [string]$Region = "westus",
        [Parameter(Position=4, Mandatory=$false)] [string]$TemplateFolderPath = "$PSScriptRoot\..\Data\ARMTemplate",
        [Parameter(Position=5, Mandatory=$false)] [string]$ParameterOutputPath = "$($(Get-Location).Path)\Parameters",
        [Parameter(Position=6, Mandatory=$false)] [string]$RestSourcePath = "$PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip",
        [ValidateSet("Developer", "Basic", "Enhanced")]
        [Parameter(Position=7, Mandatory=$false)] [string]$ImplementationPerformance = "Basic",
        [Parameter()] [switch]$ShowConnectionInstructions
    )
    BEGIN
    {
        if($ImplementationPerformance -eq "Developer") {
            Write-Warning "The ""Developer"" build creates the Azure Cosmos DB Account with the ""Free-tier"" option selected which offset the total cost. Only 1 Cosmos DB Account per tenant can make use of this tier.`n"
        }
    }
    PROCESS
    {
        ###############################
        ## Check input paths
        if(!$(Test-Path -Path $TemplateFolderPath)) {
            throw "REST Source Function Code is missing in specified path ($TemplateFolderPath)"
        }
        if(!$(Test-Path -Path $RestSourcePath)) {
            throw "REST Source Function Code is missing in specified path ($RestSourcePath)"
        }

        ###############################
        ## Create folder for the Parameter output path
        $Result = New-Item -ItemType Directory -Path $ParameterOutputPath -Force
        if($Result) {
            Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($Result.FullName))."
        }
        else {
            throw "Failed to create ARM parameters output path. Path: $ParameterOutputPath"
        }

        ###############################
        ## Connects to Azure, if not already connected.
        Write-Information "Testing connection to Azure."
        $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
        if(!($Result)) {
            throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        }

        ###############################
        ## Creates the ARM files
        $ARMObjects = New-ARMParameterObject -ParameterFolderPath $ParameterOutputPath -TemplateFolderPath $TemplateFolderPath -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance

        ###############################
        ## Create Resource Group
        Write-Information "Creating the Resource Group used to host the Windows Package Manager REST source. Name: $ResourceGroup, Region: $Region"
        Add-AzureResourceGroup -Name $ResourceGroup -Region $Region

        #### Verifies ARM Parameters are correct
        $Result = Test-ARMTemplate -ARMObjects $ARMObjects -ResourceGroup $ResourceGroup
        if($Result){
            $ErrReturnObject = @{
                ARMObjects    = $ARMObjects
                ResourceGroup = $ResourceGroup
                Result        = $Result
            }

            Write-Error -Message "Testing found an error with the ARM template or parameter files. Error: $err" -TargetObject $ErrReturnObject
        }

        ###############################
        ## Creates Azure Objects with ARM Templates and Parameters
        New-ARMObjects -ARMObjects $ARMObjects -RestSourcePath $RestSourcePath -ResourceGroup $ResourceGroup

        ###############################
        ## Shows how to connect local Windows Package Manager Client to newly created REST source
        if($ShowConnectionInstructions) {
            $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
            $AzFunctionName     = $jsonFunction.Parameters.FunctionName.Value
            $AzFunctionURL      = $(Get-AzFunctionApp -Name $AzFunctionName -ResourceGroupName $ResourceGroup).DefaultHostName

            ## Post script Run Informational:
            #### Instructions on how to add the REST source to your Windows Package Manager Client
            Write-Information -MessageData "Use the following command to register the new REST source with your Windows Package Manager Client:"
            Write-Information -MessageData "  winget source add -n ""restsource"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest"""
            Write-Verbose -Message "Use the following command to register the new REST source with your Windows Package Manager Client:"
            Write-Verbose -Message "  winget source add -n ""restsource"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest"""

            #### For more information about how to use the solution, visit the aka.ms link.
            Write-Information -MessageData "`n  For more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n"
        }
    }
    END
    {
        return $true
    }
}
