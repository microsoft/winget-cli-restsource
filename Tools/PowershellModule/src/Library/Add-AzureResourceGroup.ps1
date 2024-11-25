# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Add-AzureResourceGroup
{
    <#
    .SYNOPSIS
    Adds a Resource Group to the connected to Subscription.

    .DESCRIPTION
    Checks to see if the designated Resource Group name already exists, and if exists will not re-create. If
    the Resource Group doesn't exist, it will create a new Resource Group in Azure. If successful, returns a boolean.
        - True if the group was pre-existing or created successfully.
        - False if the group failed to be created.
    
    .PARAMETER Name
    Name of the Resource Group to be created.

    .PARAMETER Region
    The Region (westus, eastus, centralcanada, etc.) that the Resource Group will be created in.

    .EXAMPLE
    Add-AzureResourceGroup -Name "contosorestsource" -Region "westus"

    Assumes an active connection to Azure. Creates a new Resource Group named "contosorestsource" in the West US region.
    #>

    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$Name,
        [Parameter(Position=1, Mandatory=$true)] [string]$Region
    )
    BEGIN
    {
        $Return = $false
        
        ## Normalize resource group name
        $NormalizedName = $Name -replace "[^a-zA-Z0-9-()_.]", ""
        if($Name -cne $NormalizedName) {
            $Name = $NormalizedName
            Write-Warning "Removed special characters from the Azure Resource Group Name (New Name: $Name)."
        }
    }
    PROCESS
    {
        ## Creating a line break from previous steps
        Write-Verbose "Azure Resource Group Name to be created: $Name"

        ## Determines if the Resource Group already exists
        Write-Verbose "Retrieving details from Azure for the Resource Group Name $Name"
        $Result = Get-AzResourceGroup -Name $Name -ErrorAction SilentlyContinue -ErrorVariable ErrorGet -InformationAction SilentlyContinue -WarningAction SilentlyContinue

        if(!$Result) {
            Write-Information "Failed to retrieve Resource Group, will attempt to create $Name in the specified $Region."
            
            $Result = New-AzResourceGroup -Name $Name -Location $Region
            if($Result) {
                Write-Information "Resource Group $Name has been created in the $Region region."
                $Return = $true
            }
            else {
                Write-Error "Failed to retrieve or create Resource Group with name $Name."
            }
        }
        else {
            ## Found an existing Resource Group matching the name of $Name
            Write-Warning "Found an existing Resource Group matching the name of $Name. Will not create a new Resource Group."
            $Return = $true
        }
    }
    END
    {
        Return $Return
    }
}