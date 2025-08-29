# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Update-WinGetSource {
    <#
    .SYNOPSIS
    Updates a Windows Package Manager REST source in Azure.

    .DESCRIPTION
    Updates a Windows Package Manager REST source in Azure.
    The update operation will not be able to detect existing Windows Package Manager REST source configurations by default.
    To use existing configurations, put existing ARM Template Parameter files in ParameterOutputPath.

    .PARAMETER Name
    [Optional] The base name of Azure Resources (Windows Package Manager REST source components) that will be created. Required if not PublishAzureFunctionOnly.

    .PARAMETER ResourceGroup
    [Optional] The name of the Resource Group that the Windows Package Manager REST source will reside. All Azure
    Resources will be updated in in this Resource Group (Default: WinGetRestSource)

    .PARAMETER SubscriptionName
    [Optional] The name of the subscription that will be used to host the Windows Package Manager REST source. (Default: Current connected subscription)

    .PARAMETER TemplateFolderPath
    [Optional] The directory containing required ARM templates. (Default: $PSScriptRoot\..\Data\ARMTemplates)

    .PARAMETER ParameterOutputPath
    [Optional] The directory containing ARM Template Parameter files. If existing ARM Template Parameter files are found, they will be used for Windows Package Manager REST source update without modification.
    If ARM Template Parameter files are not found, new ARM Template Parameter files with default values will be created. (Default: Current Directory\Parameters)

    .PARAMETER RestSourcePath
    [Optional] Path to the compiled Azure Function (Windows Package Manager REST source) Zip file. (Default: $PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip)

    .PARAMETER PublisherName
    [Optional] The WinGet rest source publisher name. (Default: Signed in user email Or WinGetRestSource@DomainName)

    .PARAMETER PublisherEmail
    [Optional] The WinGet rest source publisher email. (Default: Signed in user email Or WinGetRestSource@DomainName)

    .PARAMETER ImplementationPerformance
    [Optional] ["Developer", "Basic", "Enhanced"] specifies the performance of the Azure Resources to be created for the Windows Package Manager REST source.
    | Preference | Description                                                                                                             |
    |------------|-------------------------------------------------------------------------------------------------------------------------|
    | Developer  | Specifies lowest cost for developing the Windows Package Manager REST source. Uses free-tier options when available.    |
    | Basic      | Specifies a basic functioning Windows Package Manager REST source.                                                      |
    | Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers.                               |

    Note for updating Windows Package Manager REST source, performance downgrading is not allowed by Azure Resources used in Windows Package Manager REST source.

    (Default: Basic)

    .PARAMETER RestSourceAuthentication
    [Optional] ["None", "MicrosoftEntraId"] The Windows Package Manager REST source authentication type. (Default: None)

    .PARAMETER MicrosoftEntraIdResource
    [Optional] Microsoft Entra Id authentication resource. (Default: None)

    .PARAMETER MicrosoftEntraIdResourceScope
    [Optional] Microsoft Entra Id authentication resource scope. (Default: None)

    .PARAMETER MaxRetryCount
    [Optional] Max ARM Templates deployment retry count upon failure. (Default: 3)

    .PARAMETER FunctionName
    [Optional] The Azure Function name. Required if PublishAzureFunctionOnly is specified. (Default: None)

    .PARAMETER PublishAzureFunctionOnly
    [Optional] If specified, only performs Azure Function publish. (Default: False)

    .EXAMPLE
    Update-WinGetSource -Name "contosorestsource" -InformationAction Continue -Verbose

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance.

    .EXAMPLE
    Update-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -SubscriptionName "Visual Studio Subscription" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic" -InformationAction Continue -Verbose

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance in the "Visual Studio Subscription" subscription.

    .EXAMPLE
    Update-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -RestSourceAuthentication "MicrosoftEntraId" -MicrosoftEntraIdResource "GUID" -MicrosoftEntraIdResourceScope "user-impersonation" -InformationAction Continue -Verbose

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance.
    Uses existing Microsoft Entra Id app registration.

    .EXAMPLE
    Update-WinGetSource -Name "contosorestsource" -PublishAzureFunctionOnly -InformationAction Continue -Verbose

    Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with publishing Azure Function only.

    #>
    PARAM(
        [Parameter(Mandatory = $false)] [string]$Name,
        [Parameter(Mandatory = $false)] [string]$ResourceGroup = 'WinGetRestSource',
        [Parameter(Mandatory = $false)] [string]$SubscriptionName = '',
        [Parameter(Mandatory = $false)] [string]$TemplateFolderPath = "$PSScriptRoot\..\Data\ARMTemplates",
        [Parameter(Mandatory = $false)] [string]$ParameterOutputPath = "$($(Get-Location).Path)\Parameters",
        [Parameter(Mandatory = $false)] [string]$RestSourcePath = "$PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip",
        [Parameter(Mandatory = $false)] [string]$PublisherName = '',
        [Parameter(Mandatory = $false)] [string]$PublisherEmail = '',
        [ValidateSet('Developer', 'Basic', 'Enhanced')]
        [Parameter(Mandatory = $false)] [string]$ImplementationPerformance = 'Basic',
        [ValidateSet('None', 'MicrosoftEntraId')]
        [Parameter(Mandatory = $false)] [string]$RestSourceAuthentication = 'None',
        [Parameter(Mandatory = $false)] [string]$MicrosoftEntraIdResource = '',
        [Parameter(Mandatory = $false)] [string]$MicrosoftEntraIdResourceScope = '',
        [Parameter(Mandatory = $false)] [int]$MaxRetryCount = 3,
        [Parameter(Mandatory = $false)] [string]$FunctionName = '',
        [Parameter()] [switch]$PublishAzureFunctionOnly
    )

    $TemplateFolderPath = [System.IO.Path]::GetFullPath($TemplateFolderPath, $pwd.Path)
    $ParameterOutputPath = [System.IO.Path]::GetFullPath($ParameterOutputPath, $pwd.Path)
    $RestSourcePath = [System.IO.Path]::GetFullPath($RestSourcePath, $pwd.Path)

    ## Check input paths
    if (!$(Test-Path -Path $TemplateFolderPath)) {
        Write-Error "REST Source Function Code is missing in specified path ($TemplateFolderPath)"
        return $false
    }
    if (!$(Test-Path -Path $RestSourcePath)) {
        Write-Error "REST Source Function Code is missing in specified path ($RestSourcePath)"
        return $false
    }

    ## Create folder for the Parameter output path
    $Result = New-Item -ItemType Directory -Path $ParameterOutputPath -Force
    if ($Result) {
        Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($Result.FullName))."
    } else {
        Write-Error "Failed to create ARM parameter files output path. Path: $ParameterOutputPath"
        return $false
    }

    ## Connects to Azure, if not already connected.
    Write-Information 'Validating connection to azure, will attempt to connect if not already connected.'
    $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
    if (!($Result)) {
        Write-Error 'Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials.'
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
            Write-Error 'FunctionName is null or empty'
            return $false
        }

        Write-Information -MessageData 'Publishing function files to the Azure Function.'
        $DeployResult = Publish-AzWebApp -ArchivePath $RestSourcePath -ResourceGroupName $ResourceGroup -Name $FunctionName -Force -ErrorVariable DeployError

        ## Verifies that no error occured when publishing the Function App
        if ($DeployError -or !$DeployResult) {
            $ErrReturnObject = @{
                DeployError  = $DeployError
                DeployResult = $DeployResult
            }

            Write-Error "Failed to publishing the Function App. $DeployError" -TargetObject $ErrReturnObject
            return $false
        }

        ## Restart the Function App
        Write-Verbose 'Restarting Azure Function.'
        if (!$(Restart-AzFunctionApp -Name $FunctionName -ResourceGroupName $ResourceGroup -Force -PassThru)) {
            Write-Error "Failed to restart Function App. Name: $FunctionName"
            return $false
        }
    } else {
        if ([string]::IsNullOrWhiteSpace($Name)) {
            Write-Error 'Name is null or empty'
            return $false
        }

        ## Check Microsoft Entra Id input
        if ($RestSourceAuthentication -eq 'MicrosoftEntraId' -and !$MicrosoftEntraIdResource) {
            Write-Error 'When Microsoft Entra Id authentication is requested, MicrosoftEntraIdResource should be provided.'
            return $false
        }

        ## Creates the ARM files
        $ARMObjects = New-ARMParameterObjects -ParameterFolderPath $ParameterOutputPath -TemplateFolderPath $TemplateFolderPath -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance -PublisherName $PublisherName -PublisherEmail $PublisherEmail -RestSourceAuthentication $RestSourceAuthentication -MicrosoftEntraIdResource $MicrosoftEntraIdResource -MicrosoftEntraIdResourceScope $MicrosoftEntraIdResourceScope -ForUpdate
        if (!$ARMObjects) {
            Write-Error 'Failed to create ARM parameter objects.'
            return $false
        }

        ## Verifies ARM Parameters are correct
        $Result = Test-ARMTemplates -ARMObjects $ARMObjects -ResourceGroup $ResourceGroup
        if ($Result) {
            $ErrReturnObject = @{
                ARMObjects    = $ARMObjects
                ResourceGroup = $ResourceGroup
                Result        = $Result
            }

            Write-Error -Message "Testing found an error with the ARM template or parameter files. Error: $err" -TargetObject $ErrReturnObject
            return $false
        }

        ## Creates Azure Objects with ARM Templates and Parameters
        $Attempt = 0
        $Retry = $false
        do {
            $Attempt++
            $Retry = $false

            $Result = New-ARMObjects -ARMObjects ([ref]$ARMObjects) -RestSourcePath $RestSourcePath -ResourceGroup $ResourceGroup
            if (!$Result) {
                if ($Attempt -lt $MaxRetryCount) {
                    $Retry = $true
                    Write-Verbose 'Retrying deployment after 15 seconds.'
                    Start-Sleep -Seconds 15
                } else {
                    Write-Error 'Failed to create Azure resources for WinGet rest source.'
                    return $false
                }
            }
        } while ($Retry)
    }

    return $true
}
