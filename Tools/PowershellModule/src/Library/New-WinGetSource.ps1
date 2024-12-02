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
        [Parameter(Position=7, Mandatory=$false)] [string]$PublisherName = "",
        [Parameter(Position=8, Mandatory=$false)] [string]$PublisherEmail = "",
        [ValidateSet("Developer", "Basic", "Enhanced")]
        [Parameter(Position=9, Mandatory=$false)] [string]$ImplementationPerformance = "Basic",
        [ValidateSet("None", "MicrosoftEntraId")]
        [Parameter(Position=10,Mandatory=$false)] [string]$RestSourceAuthentication = "None",
        [Parameter()] [switch]$CreateNewMicrosoftEntraIdAppRegistration,
        [Parameter(Position=11,Mandatory=$false)] [string]$MicrosoftEntraIdResource = "",
        [Parameter(Position=12,Mandatory=$false)] [string]$MicrosoftEntraIdResourceScope = "",
        [Parameter()] [switch]$ShowConnectionInstructions
    )
    
    if($ImplementationPerformance -eq "Developer") {
        Write-Warning "The ""Developer"" build creates the Azure Cosmos DB Account with the ""Free-tier"" option selected which offset the total cost. Only 1 Cosmos DB Account per tenant can make use of this tier.`n"
    }
    
    ###############################
    ## Check input paths
    if(!$(Test-Path -Path $TemplateFolderPath)) {
        Write-Error "REST Source Function Code is missing in specified path ($TemplateFolderPath)"
        return $false
    }
    if(!$(Test-Path -Path $RestSourcePath)) {
        Write-Error "REST Source Function Code is missing in specified path ($RestSourcePath)"
        return $false
    }
    
    ###############################
    ## Check Microsoft Entra Id input
    if ($RestSourceAuthentication -eq "MicrosoftEntraId" -and !CreateNewMicrosoftEntraIdAppRegistration -and !MicrosoftEntraIdResource) {
        Write-Error "When Microsoft Entra Id authentication is requested, either CreateNewMicrosoftEntraIdAppRegistration should be requested or MicrosoftEntraIdResource should be provided."
        return $false
    }

    ###############################
    ## Create folder for the Parameter output path
    $Result = New-Item -ItemType Directory -Path $ParameterOutputPath -Force
    if($Result) {
        Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($Result.FullName))."
    }
    else {
        Write-Error "Failed to create ARM parameter files output path. Path: $ParameterOutputPath"
        return $false
    }

    ###############################
    ## Connects to Azure, if not already connected.
    Write-Information "Testing connection to Azure."
    $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
    if(!($Result)) {
        Write-Error "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        return $false
    }

    ###############################
    ## Create new Microsoft Entra Id app registration if requested
    if ($RestSourceAuthentication -eq "MicrosoftEntraId" -and $CreateNewMicrosoftEntraIdAppRegistration) {
        $Result = New-MicrosoftEntraIdApp -Name $Name
        if (!$Result.Result) {
            Write-Error "Failed to create new Microsoft Entra Id app registration."
            return $false
        }
        else {
            $MicrosoftEntraIdResource = !$Result.Resource
            $MicrosoftEntraIdResourceScope = !$Result.ResourceScope
        }
    }

    ###############################
    ## Creates the ARM files
    $ARMObjects = New-ARMParameterObjects -ParameterFolderPath $ParameterOutputPath -TemplateFolderPath $TemplateFolderPath -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance
    if (!$ARMObjects) {
        Write-Error "Failed to create ARM parameter objects."
        return $false
    }

    ###############################
    ## Create Resource Group
    Write-Information "Creating the Resource Group used to host the Windows Package Manager REST source. Name: $ResourceGroup, Region: $Region"
    $Result = Add-AzureResourceGroup -Name $ResourceGroup -Region $Region
    if (!$Result) {
        Write-Error "Failed to create Azure resource group. Name: $ResourceGroup Region: $Region"
        return $false
    }

    #### Verifies ARM Parameters are correct
    $Result = Test-ARMTemplates -ARMObjects $ARMObjects -ResourceGroup $ResourceGroup
    if($Result){
        $ErrReturnObject = @{
            ARMObjects    = $ARMObjects
            ResourceGroup = $ResourceGroup
            Result        = $Result
        }

        Write-Error -Message "Testing found an error with the ARM template or parameter files. Error: $err" -TargetObject $ErrReturnObject
        return $false
    }

    ###############################
    ## Creates Azure Objects with ARM Templates and Parameters
    $Result = New-ARMObjects -ARMObjects $ARMObjects -RestSourcePath $RestSourcePath -ResourceGroup $ResourceGroup
    if (!$Result) {
        Write-Error "Failed to create Azure resources for WinGet rest source"
        return $false
    }

    ###############################
    ## Shows how to connect local Windows Package Manager Client to newly created REST source
    if($ShowConnectionInstructions) {
        $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
        $AzFunctionName     = $jsonFunction.Parameters.FunctionName.Value
        $AzFunctionURL      = $(Get-AzFunctionApp -Name $AzFunctionName -ResourceGroupName $ResourceGroup).DefaultHostName

        ## Post script Run Informational:
        #### Instructions on how to add the REST source to your Windows Package Manager Client
        Write-Information -MessageData "Use the following command to register the new REST source with your Windows Package Manager Client:" -InformationAction Continue
        Write-Information -MessageData "  winget source add -n ""restsource"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest""" -InformationAction Continue

        #### For more information about how to use the solution, visit the aka.ms link.
        Write-Information -MessageData "`nFor more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n" -InformationAction Continue
    }
    
    return $true
}
