# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Update-WinGetSource
{
    <#
    .SYNOPSIS
    Updates a Windows Package Manager REST source in Azure for the storage of Windows Package Manager package Manifests.

    .DESCRIPTION
    Updates a Windows Package Manager REST source in Azure for the storage of Windows Package Manager package Manifests.

    .PARAMETER Name
    The name of the objects that will be updated

    .PARAMETER ResourceGroup
    [Optional] The name of the Resource Group that the Windows Package Manager REST source will reside. All Azure
    resources will be updated in in this Resource Group (Default: WinGetRestsource)

    .PARAMETER SubscriptionName
    [Optional] The name of the subscription that will be used to host the Windows Package Manager REST source. (Default: Current connected subscription)

    .PARAMETER TemplateFolderPath
    [Optional] The directory containing required ARM templates. (Default: $PSScriptRoot\..\Data\ARMTemplates)
    
    .PARAMETER ParameterOutputPath
    [Optional] The directory containing Parameter files. If existing Parameter files are found, they will be used for WinGetRestSource update without modification.
    If Parameter files are not found, new Parameter files with default values will be created. (Default: Current Directory\Parameters)

    .PARAMETER RestSourcePath
    [Optional] Path to the compiled REST API Zip file. (Default: $PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip)

    .PARAMETER PublisherName
    [Optional] The WinGet rest source publisher name

    .PARAMETER PublisherEmail
    [Optional] The WinGet rest source publisher email

    .PARAMETER ImplementationPerformance
    [Optional] ["Developer", "Basic", "Enhanced"] specifies the performance of the resources to be created for the Windows Package Manager REST source.
    | Preference | Description                                                                                                             |
    |------------|-------------------------------------------------------------------------------------------------------------------------|
    | Developer  | Specifies lowest cost for developing the Windows Package Manager REST source. Uses free-tier options when available.    |
    | Basic      | Specifies a basic functioning Windows Package Manager REST source.                                                      |
    | Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers.                               |

    Note for updating WinGet source, performance downgrading is not allowed by Azure resources used in WinGet source.

    (Default: Basic)

    .PARAMETER RestSourceAuthentication
    [Optional] ["None", "MicrosoftEntraId"] The WinGet rest source authentication type. [Default: None]

    .PARAMETER MicrosoftEntraIdResource
    [Optional] Microsoft Entra Id authentication resource

    .PARAMETER MicrosoftEntraIdResourceScope
    [Optional] Microsoft Entra Id authentication resource scope

    .PARAMETER FunctionName
    [Optional] The Azure Function name. Required if PublishAzureFunctionOnly is specified.

    .PARAMETER PublishAzureFunctionOnly
    [Optional] If specified, only does Azure Function publish.

    .EXAMPLE
    Update-WinGetSource-WinGetSource -Name "contosorestsource"

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance.

    .EXAMPLE
    Update-WinGetSource-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -SubscriptionName "Visual Studio Subscription" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic"

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance in the "Visual Studio Subscription" Subscription.

    .EXAMPLE
    Update-WinGetSource-WinGetSource -Name "contosorestsource" -PublishAzureFunctionOnly

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with publishing Azure Function only.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$Name,
        [Parameter(Mandatory=$false)] [string]$ResourceGroup = "WinGetRestSource",
        [Parameter(Mandatory=$false)] [string]$SubscriptionName = "",
        [Parameter(Mandatory=$false)] [string]$TemplateFolderPath = "$PSScriptRoot\..\Data\ARMTemplates",
        [Parameter(Mandatory=$false)] [string]$ParameterOutputPath = "$($(Get-Location).Path)\Parameters",
        [Parameter(Mandatory=$false)] [string]$RestSourcePath = "$PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip",
        [Parameter(Mandatory=$false)] [string]$PublisherName = "",
        [Parameter(Mandatory=$false)] [string]$PublisherEmail = "",
        [ValidateSet("Developer", "Basic", "Enhanced")]
        [Parameter(Mandatory=$false)] [string]$ImplementationPerformance = "Basic",
        [ValidateSet("None", "MicrosoftEntraId")]
        [Parameter(Mandatory=$false)] [string]$RestSourceAuthentication = "None",
        [Parameter(Mandatory=$false)] [string]$MicrosoftEntraIdResource = "",
        [Parameter(Mandatory=$false)] [string]$MicrosoftEntraIdResourceScope = "",
        [Parameter(Mandatory=$false)] [string]$FunctionName = "",
        [Parameter()] [switch]$PublishAzureFunctionOnly
    )

    $TemplateFolderPath = [System.IO.Path]::GetFullPath($TemplateFolderPath, $pwd.Path)
    $ParameterOutputPath = [System.IO.Path]::GetFullPath($ParameterOutputPath, $pwd.Path)
    $RestSourcePath = [System.IO.Path]::GetFullPath($RestSourcePath, $pwd.Path)

    ## Check input paths
    if(!$(Test-Path -Path $TemplateFolderPath)) {
        Write-Error "REST Source Function Code is missing in specified path ($TemplateFolderPath)"
        return $false
    }
    if(!$(Test-Path -Path $RestSourcePath)) {
        Write-Error "REST Source Function Code is missing in specified path ($RestSourcePath)"
        return $false
    }

    ## Create folder for the Parameter output path
    $Result = New-Item -ItemType Directory -Path $ParameterOutputPath -Force
    if($Result) {
        Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($Result.FullName))."
    }
    else {
        Write-Error "Failed to create ARM parameter files output path. Path: $ParameterOutputPath"
        return $false
    }

    ## Connects to Azure, if not already connected.
    Write-Information "Testing connection to Azure."
    $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
    if(!($Result)) {
        Write-Error "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        return $false
    }

    ## Check resource group exists
    $GetResult = Get-AzResourceGroup -Name $ResourceGroup
    if (!$GetResult) {
        Write-Error "Failed to get Azure Resource Group. Name: $ResourceGroup"
        return $false
    }
    $Region = $GetResult.Location

    if ($PublishAzureFunctionOnly) {
        if ([string]::IsNullOrWhiteSpace($FunctionName)) {
            Write-Error "FunctionName is null or empty"
            return $false
        }

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
        Write-Verbose "Restarting Azure Function."
        Restart-AzFunctionApp -Name $FunctionName -ResourceGroupName $ResourceGroup -Force
    }
    else {
        ## Check Microsoft Entra Id input
        if ($RestSourceAuthentication -eq "MicrosoftEntraId" -and !$MicrosoftEntraIdResource) {
            Write-Error "When Microsoft Entra Id authentication is requested, MicrosoftEntraIdResource should be provided."
            return $false
        }

        ## Creates the ARM files
        $ARMObjects = New-ARMParameterObjects -ParameterFolderPath $ParameterOutputPath -TemplateFolderPath $TemplateFolderPath -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance -PublisherName $PublisherName -PublisherEmail $PublisherEmail -RestSourceAuthentication $RestSourceAuthentication -MicrosoftEntraIdResource $MicrosoftEntraIdResource -MicrosoftEntraIdResourceScope $MicrosoftEntraIdResourceScope -ForUpdate
        if (!$ARMObjects) {
            Write-Error "Failed to create ARM parameter objects."
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

        ## Creates Azure Objects with ARM Templates and Parameters
        $Result = New-ARMObjects -ARMObjects $ARMObjects -RestSourcePath $RestSourcePath -ResourceGroup $ResourceGroup
        if (!$Result) {
            Write-Error "Failed to create Azure resources for WinGet rest source"
            return $false
        }
    }

    return $true
}
