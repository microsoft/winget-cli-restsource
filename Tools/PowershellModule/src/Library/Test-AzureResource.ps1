# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-AzureResource
{
    <#
    .SYNOPSIS
    Returns a boolean result validating that the Resource Group and Resource exist.

    .DESCRIPTION
    Returns a boolean result validating that the Resource Group and Resource exist.

    .PARAMETER ResourceGroup
    The Resource Group that the objects will be tested in reference to.

    .PARAMETER ResourceName
    Name of the Azure Resource name.

    .EXAMPLE
    Test-AzureResource -ResourceGroup "WinGet" -ResourceName "contosorestsource"

    Returns a boolean result validating that the Resource Group and Resource exist.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory = $true)] [string]$ResourceGroup,
        [Parameter(Position=1, Mandatory = $true)] [string]$ResourceName,
        [ValidateSet("Function")]
        [Parameter(Position=2, Mandatory = $false)] [string]$ResourceType = "Function"
    )
    BEGIN
    {
        $Result = $false
        $AzureResourceGroupName = $ResourceGroup
        $AzureResourceName = $ResourceName

        $AzureResourceGroupNameNullOrWhiteSpace = $false
        $AzureResourceGroupNameExists           = $false
        
        $AzureResourceNameNullOrWhiteSpace = $false
        $AzureResourceNameExists           = $false
        
        ##Determines if the Azure Resource Group Name is not null or empty
        if([string]::IsNullOrWhiteSpace($AzureResourceGroupName)) {
            $AzureResourceGroupName                 = "<null>"
            $AzureResourceGroupNameNullOrWhiteSpace = $true 
        }

        if($(Get-AzResourceGroup).Where({$_.ResourceGroupName -eq $AzureResourceGroupName}).Count -gt 0) {
            $AzureResourceGroupNameExists = $true
        }

        ## Determines if the Azure Resource Name is not null or empty
        if([string]::IsNullOrWhiteSpace($AzureResourceName)) {
            $AzureResourceName               = "<null>"
            $AzureResourceNameNullOrWhiteSpace = $true 
        }
    
        ## Determines if the Azure Resource Name is in Azure
        if ($AzureResourceGroupNameExists) {
            switch ($ResourceType) {
                "Function" {
                    if($(Get-AzFunctionApp -ResourceGroupName $AzureResourceGroupName).Where({$_.Name -eq $AzureResourceName}).Count -gt 0) {
                        $AzureResourceNameExists = $true
                    }
                }
            }
        }
    }
    PROCESS
    {
        $VerboseMessage =  "Azure Resources:`n"
        $VerboseMessage += "           Azure Resource Group Exists: $AzureResourceGroupNameExists`n"
        $VerboseMessage += "           Azure Resource Exists:       $AzureResourceNameExists"
        Write-Verbose -Message $VerboseMessage

        ## If either the Azure Resource Name or the Azure Resource Group Name are null, error.
        if($AzureResourceGroupNameNullOrWhiteSpace -or $AzureResourceNameNullOrWhiteSpace -or !$AzureResourceGroupNameExists -or !$AzureResourceNameExists) {
            $ErrorMessage = "Both the Azure Resource Group and Resource Names can not be null and must exist. Please verify that the Azure Resource Group and Resource exist."
            $ErrReturnObject = @{
                AzureResourceGroupNameNullOrWhiteSpace = $AzureResourceGroupNameNullOrWhiteSpace
                AzureResourceNameNullOrWhiteSpace      = $AzureResourceNameNullOrWhiteSpace
                AzureResourceGroupNameExists         = $AzureResourceGroupNameExists
                AzureResourceNameExists              = $AzureResourceNameExists
                Result                               = $false
            }

            Write-Error -Message $ErrorMessage -Category InvalidArgument -TargetObject $ErrReturnObject
        }
        
        $Result = $AzureResourceGroupNameExists -and $AzureResourceNameExists
    }
    END
    {
        return $Result
    }
}