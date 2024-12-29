# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-WinGetSource {
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
    [Optional] The directory containing required ARM templates. (Default: $PSScriptRoot\..\Data\ARMTemplates)
    
    .PARAMETER ParameterOutputPath
    [Optional] The directory where Parameter files will be created in. (Default: Current Directory\Parameters)

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

    (Default: Basic)

    .PARAMETER RestSourceAuthentication
    [Optional] ["None", "MicrosoftEntraId"] The WinGet rest source authentication type. [Default: None]

    .PARAMETER CreateNewMicrosoftEntraIdAppRegistration
    [Optional] If specified, a new Microsoft Entra Id app registration will be created. (Default: False)

    .PARAMETER MicrosoftEntraIdResource
    [Optional] Microsoft Entra Id authentication resource

    .PARAMETER MicrosoftEntraIdResourceScope
    [Optional] Microsoft Entra Id authentication resource scope

    .PARAMETER ShowConnectionInstructions
    [Optional] If specified, the instructions for connecting to the Windows Package Manager REST source. (Default: False)

    .PARAMETER MaxRetryCount
    [Optional] Max ARM template deployment retry count upon failure. (Default: 5)

    .EXAMPLE
    New-WinGetSource -Name "contosorestsource" -InformationAction Continue -Verbose

    Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of
    Azure with the basic level performance.

    .EXAMPLE
    New-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -SubscriptionName "Visual Studio Subscription" -Region "westus" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic" -ShowConnectionInformation -InformationAction Continue -Verbose

    Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of
    Azure with the basic level performance in the "Visual Studio Subscription" Subscription. Displays the required command
    to connect the WinGet client to the new REST source after the REST source has been created.

    #>
    PARAM(
        [Parameter(Position = 0, Mandatory = $true)] [string]$Name,
        [Parameter(Mandatory = $false)] [string]$ResourceGroup = 'WinGetRestSource',
        [Parameter(Mandatory = $false)] [string]$SubscriptionName = '',
        [Parameter(Mandatory = $false)] [string]$Region = 'westus',
        [Parameter(Mandatory = $false)] [string]$TemplateFolderPath = "$PSScriptRoot\..\Data\ARMTemplates",
        [Parameter(Mandatory = $false)] [string]$ParameterOutputPath = "$($(Get-Location).Path)\Parameters",
        [Parameter(Mandatory = $false)] [string]$RestSourcePath = "$PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip",
        [Parameter(Mandatory = $false)] [string]$PublisherName = '',
        [Parameter(Mandatory = $false)] [string]$PublisherEmail = '',
        [ValidateSet('Developer', 'Basic', 'Enhanced')]
        [Parameter(Mandatory = $false)] [string]$ImplementationPerformance = 'Basic',
        [ValidateSet('None', 'MicrosoftEntraId')]
        [Parameter(Mandatory = $false)] [string]$RestSourceAuthentication = 'None',
        [Parameter()] [switch]$CreateNewMicrosoftEntraIdAppRegistration,
        [Parameter(Mandatory = $false)] [string]$MicrosoftEntraIdResource = '',
        [Parameter(Mandatory = $false)] [string]$MicrosoftEntraIdResourceScope = '',
        [Parameter()] [switch]$ShowConnectionInstructions,
        [Parameter(Mandatory = $false)] [int]$MaxRetryCount = 5
    )

    if ($ImplementationPerformance -eq 'Developer') {
        Write-Warning "The ""Developer"" build creates the Azure Cosmos DB Account with the ""Free-tier"" option selected which offset the total cost. Only 1 Cosmos DB Account per tenant can make use of this tier.`n"
    }

    $TemplateFolderPath = [System.IO.Path]::GetFullPath($TemplateFolderPath, $pwd.Path)
    $ParameterOutputPath = [System.IO.Path]::GetFullPath($ParameterOutputPath, $pwd.Path)
    $RestSourcePath = [System.IO.Path]::GetFullPath($RestSourcePath, $pwd.Path)

    ###############################
    ## Check input paths
    if (!$(Test-Path -Path $TemplateFolderPath)) {
        Write-Error "REST Source Function Code is missing in specified path ($TemplateFolderPath)"
        return $false
    }
    if (!$(Test-Path -Path $RestSourcePath)) {
        Write-Error "REST Source Function Code is missing in specified path ($RestSourcePath)"
        return $false
    }

    ###############################
    ## Check Microsoft Entra Id input
    if ($RestSourceAuthentication -eq 'MicrosoftEntraId' -and !$CreateNewMicrosoftEntraIdAppRegistration -and !$MicrosoftEntraIdResource) {
        Write-Error 'When Microsoft Entra Id authentication is requested, either CreateNewMicrosoftEntraIdAppRegistration should be requested or MicrosoftEntraIdResource should be provided.'
        return $false
    }

    ###############################
    ## Create folder for the Parameter output path
    $Result = New-Item -ItemType Directory -Path $ParameterOutputPath -Force
    if ($Result) {
        Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($Result.FullName))."
    } else {
        Write-Error "Failed to create ARM parameter files output path. Path: $ParameterOutputPath"
        return $false
    }

    ###############################
    ## Connects to Azure, if not already connected.
    Write-Information 'Validating connection to azure, will attempt to connect if not already connected.'
    $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
    if (!($Result)) {
        Write-Error 'Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials.'
        return $false
    }

    ###############################
    ## Create new Microsoft Entra Id app registration if requested
    if ($RestSourceAuthentication -eq 'MicrosoftEntraId' -and $CreateNewMicrosoftEntraIdAppRegistration) {
        $Result = New-MicrosoftEntraIdApp -Name $Name
        if (!$Result.Result) {
            Write-Error 'Failed to create new Microsoft Entra Id app registration.'
            return $false
        } else {
            $MicrosoftEntraIdResource = $Result.Resource
            $MicrosoftEntraIdResourceScope = $Result.ResourceScope
        }
    }

    ###############################
    ## Creates the ARM files
    $ARMObjects = New-ARMParameterObjects -ParameterFolderPath $ParameterOutputPath -TemplateFolderPath $TemplateFolderPath -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance -PublisherName $PublisherName -PublisherEmail $PublisherEmail -RestSourceAuthentication $RestSourceAuthentication -MicrosoftEntraIdResource $MicrosoftEntraIdResource -MicrosoftEntraIdResourceScope $MicrosoftEntraIdResourceScope
    if (!$ARMObjects) {
        Write-Error 'Failed to create ARM parameter objects.'
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

    ###############################
    ## Verifies ARM Parameters are correct. If any failed, the return results will contain failed objects. Otherwise, success.
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

    ###############################
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

    ###############################
    ## Shows how to connect local Windows Package Manager Client to newly created REST source
    if ($ShowConnectionInstructions) {
        $ApiManagementName = $ARMObjects.Where({ $_.ObjectType -eq 'ApiManagement' }).Parameters.Parameters.serviceName.value
        $ApiManagementURL = (Get-AzApiManagement -Name $ApiManagementName -ResourceGroupName $ResourceGroup).RuntimeUrl

        ## Post script Run Informational:
        #### Instructions on how to add the REST source to your Windows Package Manager Client
        Write-Information -MessageData 'Use the following command to register the new REST source with your Windows Package Manager Client:' -InformationAction Continue
        Write-Information -MessageData "  winget source add -n ""$Name"" -a ""$ApiManagementURL/winget/"" -t ""Microsoft.Rest""" -InformationAction Continue

        #### For more information about how to use the solution, visit the aka.ms link.
        Write-Information -MessageData "`nFor more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n" -InformationAction Continue
    }

    return $true
}
