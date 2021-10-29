# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-AzureResource
{
    <#
    .SYNOPSIS
    

    .DESCRIPTION


    .PARAMETER ResourceGroup
    The Resource Group that the objects will be tested in reference to.

    .PARAMETER FunctionName
    

    .EXAMPLE
    Test-AzureResource -ResourceGroup "WinGetResourceGroup" -FunctionName "Contoso0002"



    #>
    PARAM(
        [Parameter(Position=0, Mandatory = $false)] [string]$ResourceGroup,
        [Parameter(Position=1, Mandatory = $false)] [string]$FunctionName
    )
    BEGIN
    {
        $Result = $true
        $AzureResourceGroupName = $ResourceGroup
        $AzureFunctionName = $FunctionName
        
        $AzureFunctionNameNotNullOrEmpty = $true
        $AzureFunctionNameExists         = $true

        $AzureResourceGroupNameNotNullOrEmpty = $true
        $AzureResourceGroupNameExists         = $true

        ## Determines if the Azure Function App Name is not null or empty
        if($AzureFunctionName.Length -le 0) {
            $AzureFunctionName               = "<null>"
            $AzureFunctionNameNotNullOrEmpty = $false 
        }
    
        ## Determines if the Azure Function App Name is in Azure
        if($(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).Count -le 0) {
            $AzureFunctionNameExists = $false
        }
        
        ##Determines if the Azure Resource Group Name is not null or empty
        if($AzureResourceGroupName.Length -le 0) {
            $AzureResourceGroupName               = "<null>"
            $AzureResourceGroupNameNotNullOrEmpty = $false 
        }

        if($(Get-AzResourceGroup).Where({$_.ResourceGroupName -eq $AzureResourceGroupName}).Count -lt 0) {
            $AzureResourceGroupNameExists = $false
        }
   }
    PROCESS
    {
        $VerboseMessage =  "Azure Resources:`n"
        $VerboseMessage += "           Azure Function Exists:       $AzureFunctionNameExists`n"
        $VerboseMessage += "           Azure Resource Group Exists: $AzureResourceGroupNameExists"
        Write-Verbose -Message $VerboseMessage

        ## If either the Azure Function Name or the Azure Resource Group Name are null, error.
        if(!$AzureFunctionNameNotNullOrEmpty -or !$AzureResourceGroupNameNotNullOrEmpty -or !$AzureFunctionNameExists -or !$AzureResourceGroupNameExists) {
            $ErrorMessage = "Both the Azure Function and Resource Group Names can not be null and must exist. Please verify that the Azure function and Resource Group exist."
            $ErrReturnObject = @{
                AzureFunctionNameNotNullOrEmpty      = $AzureFunctionNameNotNullOrEmpty
                AzureResourceGroupNameNotNullOrEmpty = $AzureResourceGroupNameNotNullOrEmpty
                AzureFunctionNameExists              = $AzureFunctionNameExists
                AzureResourceGroupNameExists         = $AzureResourceGroupNameExists
                Result                               = $false
            }

            Write-Error -Message $ErrorMessage -Category InvalidArgument -TargetObject $ErrReturnObject
            $Result = $false
        }
    }
    END
    {
        return $Result
    }
}