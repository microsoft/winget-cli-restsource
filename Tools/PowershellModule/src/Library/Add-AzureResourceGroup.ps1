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
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
    
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
        $ErrorMessageRGDoesNotExist = "*Provided resource group does not exist*"
        $SupportedRegions = @("eastasia", "southeastasia", "centralus", "eastus", "eastus2", "westus", "northcentralus", 
                              "southcentralus", "northeurope", "southeurope", "westeurope", "japanwest", "japaneast", 
                              "brazilsouth", "australiaeast", "australiasoutheast", "southindia", "centralindia", "westindia", 
                              "canadacentral", "canadaeast", "uksouth", "ukwest", "westcentralus", "westus2", "koreacentral", 
                              "koreasouth", "francecentral", "francesouth", "australiacentral", "australiacentral2", "uaecentral", 
                              "uaenorth", "southafricanorth", "southafricawest")

        if($Name.Contains("-")) {
            $Name = $("$Name").Replace("-","")
            Write-Information -MessageData "Removed special characters from the Azure Resource Group Name (New Name: $Name)."
        }

        if($Region -and !$SupportedRegions.Contains($Region.ToLower())) {
            ## Provided Azure Region does not match supported regions in $SupportedRegions variable.
            Write-Warning -Message "Provided Azure region $Region is not in the list of supported Azure Regions. Will attempt to create Resource Group in the provided Region."
        }
    }
    PROCESS
    {
        ## Creating a line break from previous steps
        Write-Verbose "Azure Resource Group Name to be created: $Name"

        ## Determines if the Resource Group already exists
        Write-Verbose "Retrieving details from Azure for the Resource Group Name $Name"
        $Result = Get-AzResourceGroup -Name $Name -ErrorAction SilentlyContinue -ErrorVariable err -InformationAction SilentlyContinue -WarningAction SilentlyContinue

        if(!$Result) {
            if($err.Where({$_ -like $ErrorMessageRGDoesNotExist})) {
                Write-Verbose -Message "Resource Group does not exist, will create $Name in the specified $Region."
            }
            
            $Result = New-AzResourceGroup -Name $Name -Location $Region
            if($Result) {
                Write-Information -MessageData "Resource Group $Name has been created in the $Region region."
                $Return = $true
            }
        }
        else {
            ## Found an existing Resource Group matching the name of $Name
            Write-Warning -Message "Found an existing Resource Group matching the name of $Name. Will not create a new Resource Group."
            $Return = $true
        }
    }
    END
    {
        Return $Return
    }
}