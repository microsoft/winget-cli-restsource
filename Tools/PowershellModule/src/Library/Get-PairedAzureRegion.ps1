# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.

Function Get-PairedAzureRegion
{
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$Region
    )
    BEGIN
    {
        $LocationList = Get-AzLocation
    }
    PROCESS
    {
        if($Region -like "*us*" -and $Region -notlike "*australia*") {
            switch ($Region) {
                "eastus" {
                    $Result = "westus"
                }
                "westus" {
                    $Result = "eastus"
                }
                "eastus2" {
                    $Result = "centralus"
                }
                "centralus" {
                    $Result = "eastus2"
                }
                "northcentralus" {
                    $Result = "southcentralus"
                }
                "southcentralus" {
                    $Result = "northcentralus"
                }
                "westus2" {
                    $Result = "westcentralus"
                }
                "westcentralus" {
                    $Result = "westus2"
                }
                "westus3" {
                    $Result = "westcentralus"
                }
                Default {
                    $Result = "westus"
                }
            }
        }
        elseif($Region -like "*canada*") {
            switch ($Region) {
                "canadacentral" {
                    $Result = "canadaeast"
                }
                "canadaeast" {
                    $Result = "canadacentral"
                }
                Default {
                    $Result = "canadacentral"
                }
            }
        }
        elseif($Region -like "*asia*") {
            switch ($Region) {
                "eastasia" {
                    $Result = "southeastasia"
                }
                "southeastasia" {
                    $Result = "eastasia"
                }
                Default {
                    $Result = "eastasia"
                }
            }
        }
        elseif($Region -like "*japan*") {
            switch ($Region) {
                "japanwest" {
                    $Result = "japaneast"
                }
                "japaneast" {
                    $Result = "japanwest"
                }
                Default {
                    $Result = "japanwest"
                }
            }
        }
        elseif($Region -like "*europe*") {
            switch ($Region) {
                "northeurope" {
                    $Result = "westeurope"
                }
                "westeurope" {
                    $Result = "northeurope"
                }
                Default {
                    $Result = "westeurope"
                }
            }
        }
        elseif($Region -like "*brazil*") {
            switch ($Region) {
                "brazilsouth" {
                    $Result = "brazilsoutheast"
                }
                "brazilsoutheast" {
                    $Result = "brazilsouth"
                }
                Default {
                    $Result = "brazilsoutheast"
                }
            }
        }
        elseif($Region -like "*australia*") {
            switch ($Region) {
                "australiaeast" {
                    $Result = "australiasoutheast"
                }
                "australiasoutheast" {
                    $Result = "australiaeast"
                }
                "australiacentral" {
                    $Result = "australiacentral2"
                }
                "australiacentral2" {
                    $Result = "australiacentral"
                }
                Default {
                    $Result = "australiasoutheast"
                }
            }
        }
        elseif($Region -like "*india*") {
            switch ($Region) {
                "westindia" {
                    $Result = "southindia"
                }
                "centralindia" {
                    $Result = "southindia"
                }
                "southindia" {
                    $Result = "centralindia"
                }
                Default {
                    $Result = "southindia"
                }
            }
        }
        elseif($Region -like "*uk*") {
            switch ($Region) {
                "uksouth" {
                    $Result = "ukwest"
                }
                "ukwest" {
                    $Result = "uksouth"
                }
                Default {
                    $Result = "uksouth"
                }
            }
        }
        elseif($Region -like "*korea*") {
            switch ($Region) {
                "koreacentral" {
                    $Result = "koreasouth"
                }
                "koreasouth" {
                    $Result = "koreacentral"
                }
                Default {
                    $Result = "koreacentral"
                }
            }
        }
        elseif($Region -like "*france*") {
            switch ($Region) {
                "francecentral" {
                    $Result = "francesouth"
                }
                "francesouth" {
                    $Result = "francecentral"
                }
                Default {
                    $Result = "francecentral"
                }
            }
        }
        elseif($Region -like "*africa*") {
            switch ($Region) {
                "southafricanorth" {
                    $Result = "southafricawest"
                }
                "southafricawest" {
                    $Result = "southafricanorth"
                }
                Default {
                    $Result = "southafricanorth"
                }
            }
        }
        elseif($Region -like "*switzerland*") {
            switch ($Region) {
                "switzerlandnorth" {
                    $Result = "switzerlandwest"
                }
                "switzerlandwest" {
                    $Result = "switzerlandnorth"
                }
                Default {
                    $Result = "switzerlandwest"
                }
            }
        }
        elseif($Region -like "*germany*") {
            switch ($Region) {
                "germanynorth" {
                    $Result = "germanywestcentral"
                }
                "germanywestcentral" {
                    $Result = "germanynorth"
                }
                Default {
                    $Result = "germanywestcentral"
                }
            }
        }
        elseif($Region -like "*norway*") {
            switch ($Region) {
                "norwaywest" {
                    $Result = "norwayeast"
                }
                "norwayeast" {
                    $Result = "norwaywest"
                }
                Default {
                    $Result = "norwaywest"
                }
            }
        }
        elseif($Region -like "*uae*") {
            switch ($Region) {
                "uaecentral" {
                    $Result = "uaenorth"
                }
                "uaenorth" {
                    $Result = "uaecentral"
                }
                Default {
                    $Result = "uaecentral"
                }
            }
        }
        else {            
            $Result = "westus"
        }
    }
    END
    {
        Return $Result
    }
}