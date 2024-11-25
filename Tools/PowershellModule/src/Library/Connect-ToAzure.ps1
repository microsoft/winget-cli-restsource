# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Connect-ToAzure
{
    <#
    .SYNOPSIS
    Connects to an Azure environment and connects to a specific Azure Subscription if a name of the subscription has been provided.
    
    .DESCRIPTION
    By running this function the user will be prompted to conect to their Azure environment. If a connection is not already 
    established to the (if specified) Subscription Name and / or Subscription Id.
        
    .PARAMETER SubscriptionName
    [Optional] The Subscription name that contains the Windows Package Manager REST source REST APIs

    .PARAMETER SubscriptionId
    [Optional] The Subscription Id that contains the Windows Package Manager REST source REST APIs

    .EXAMPLE
    Connect-ToAzure -SubscriptionName "Visual Studio Subscription"

    Tests that the PowerShell session has a connection to Azure, and is connected to the "Visual Studio Subscription" 
    subscription. If not connected, will initiate a connection to the Azure Subscription Name.

    .EXAMPLE
    Connect-ToAzure -SubscriptionId "5j7ty5xj-6q67-1a77-111d-1d6937b9cbe8"

    Tests that the PowerShell session has a connection to Azure, and is connected to the "5j7ty5xj-6q67-1a77-111d-1d6937b9cbe8" 
    subscription Id. If not connected, will initiate a connection to the Azure Subscription Id.

    .EXAMPLE
    Connect-ToAzure -SubscriptionName "Visual Studio Subscription" -SubscriptionId "5j7ty5xj-6q67-1a77-111d-1d6937b9cbe8"

    Tests that the PowerShell session has a connection to Azure, and is connected to the "5j7ty5xj-6q67-1a77-111d-1d6937b9cbe8" 
    subscription Id and Subscription Name "Visual Studio Subscription". If not connected, will initiate a connection to the 
    specified Azure Subscription Name and Id.     

    #>
    
    PARAM(
        [Parameter(Position=0, Mandatory=$false)] [string]$SubscriptionName,
        [Parameter(Position=1, Mandatory=$false)] [string]$SubscriptionId
    )
    BEGIN
    {
        $TestAzureConnection = $false
        
        if($SubscriptionName -and $SubscriptionId){
            ## If connected to Azure, and the Subscription Name and Id are provided then verify that the connected Azure session matches the provided Subscription Name and Id.
            Write-Verbose -Message "Verifying if PowerShell session is currently connected to your Azure Subscription Name $SubscriptionName and Subscription Id $SubscriptionId"
            $TestAzureConnection = Test-ConnectionToAzure -SubscriptionName $SubscriptionName -SubscriptionId $SubscriptionId
        }
        elseif($SubscriptionName){
            ## If connected to Azure, and the Subscription Name are provided then verify that the connected Azure session matches the provided Subscription Name.
            Write-Verbose -Message "Verifying if PowerShell session is currently connected to your Azure Subscription Name $SubscriptionName"
            $TestAzureConnection = Test-ConnectionToAzure -SubscriptionName $SubscriptionName
        }
        elseif($SubscriptionId -and $TestAzureConnection){
            ## If connected to Azure, and the Subscription Id are provided then verify that the connected Azure session matches the provided Subscription Id.
            Write-Verbose -Message "Verifying if PowerShell session is currently connected to your Azure Subscription Id $SubscriptionId"
            $TestAzureConnection = Test-ConnectionToAzure -SubscriptionId $SubscriptionId
        }
        else{
            Write-Information "No Subscription Name or Subscription Id provided. Will test connection to default Azure Subscription"
            $TestAzureConnection = Test-ConnectionToAzure
        }
        
        Write-Verbose -Message "Test Connection Result: $TestAzureConnection"
    }
    PROCESS
    {
        if(!$TestAzureConnection) {
            if($SubscriptionName -and $SubscriptionId) {
                ## Attempts a connection to Azure using both the Subscription Name and Subscription Id
                Write-Information "Initiating a connection to your Azure Subscription Name $SubscriptionName and Subscription Id $SubscriptionId"
                Connect-AzAccount -SubscriptionName $SubscriptionName -SubscriptionId $SubscriptionId
                
                $TestAzureConnection = Test-ConnectionToAzure -SubscriptionName $SubscriptionName -SubscriptionId $SubscriptionId
            }
            elseif($SubscriptionName) {
                ## Attempts a connection to Azure using Subscription Name
                Write-Information "Initiating a connection to your Azure Subscription Name $SubscriptionName"
                Connect-AzAccount -SubscriptionName $SubscriptionName

                $TestAzureConnection = Test-ConnectionToAzure -SubscriptionName $SubscriptionName
            }
            elseif($SubscriptionId) {
                ## Attempts a connection to Azure using Subscription Id
                Write-Information "Initiating a connection to your Azure Subscription Id $SubscriptionId"
                Connect-AzAccount -SubscriptionId $SubscriptionId

                $TestAzureConnection = Test-ConnectionToAzure -SubscriptionId $SubscriptionId
            }
            else{
                ## Attempts a connection to Azure with the users default Subscription
                Write-Information "Initiating a connection to your Azure environment."
                Connect-AzAccount

                $TestAzureConnection = Test-ConnectionToAzure
            }

            if(!$TestAzureConnection) {
                ## If the connection fails, or the user cancels the login request, then throw an error.
                $ErrReturnObject = @{
                    SubscriptionName = $SubscriptionName
                    SubscriptionId   = $SubscriptionId
                    AzureConnected   = $TestAzureConnection
                }

                Write-Error -Message  "Failed to connect to Azure" -TargetObject $ErrReturnObject
            }
        }
    }
    END
    {
        return $TestAzureConnection
    }
}