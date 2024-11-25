# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-ConnectionToAzure
{
    <#
    .SYNOPSIS
    Validates that a connection is existing to Azure and/or Azure Subscription Name / Id.

    .DESCRIPTION
    Validates that a connection is existing to Azure and/or Azure Subscription Name / Id.

    .PARAMETER SubscriptionName
    [Optional] Name of the Azure Subscription.

    .PARAMETER SubscriptionId
    [Optional] Id of the Azure Subscription.

    .EXAMPLE
    Test-ConnectionToAzure

    Returns a Boolean if the current session is connected to Azure.

    .EXAMPLE
    Test-ConnectionToAzure -SubscriptionName "Visual Studio Subscription"

    Returns a Boolean if the current session is connected to Azure and the active Subscription matches with the specified Subscription Name.

    .EXAMPLE
    Test-ConnectionToAzure -SubscriptionName "Visual Studio Subscription" -SubscriptionId "5j7ty5xj-6q67-1a77-111d-1d6937b9cbe8"

    Returns a Boolean if the current session is connected to Azure and the active Subscription matches with the specified Subscription Name and Id.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$false)] [string] $SubscriptionName,
        [Parameter(Position=1, Mandatory=$false)] [string] $SubscriptionId
    )
    BEGIN
    {
        $Result    = $false
        $AzContext = Get-AzContext
    }
    PROCESS
    {
        if($AzContext) {
            if($SubscriptionName -and $AzContext.Subscription.Name -ne $SubscriptionName) {
                ## If Subscription Name paramter is passed in, and the value doesn't match current connection return $false
                Write-Error "Connection to an unmatched Subscription in Azure. Not connected to $SubscriptionName"
                $Result = $false
            }
            elseif($SubscriptionId -and $AzContext.Subscription.Id -ne $SubscriptionId) {
                ## If Subscription Id paramter is passed in, and the value doesn't match current connection return $false
                Write-Error "Connection to an unmatched Subscription in Azure. Not connected to $SubscriptionId"
                $Result = $false
            }
            else {
                Write-Information "Connected to Azure"
                $Result = $true
            }
        }
        else {
            ## Not currently connected to Azure
            Write-Error "Not connected to Azure, please connect to your Azure Subscription"
            $Result = $false
        }
    }
    END
    {
        Return $Result
    }
}