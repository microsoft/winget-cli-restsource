<#
.SYNOPSIS
    Sets roles to a different azure resources.
.DESCRIPTION
    Sets the roles for different service principal for different resources.
#>
Param(
    [Parameter(Mandatory=$true)]
    [String]
    $subscriptionId,

    [Parameter(Mandatory=$true)]
    [String]
    $resourceGroup,

    [Parameter(Mandatory=$true)]
    [String]
    $rolesFile,

    [Switch]
    $showWarnings
)

enum ResourceType
{
    StorageAccount
    AppConfig
}

function GetScope {
    param (
        [Parameter(Mandatory=$true)]
        [ResourceType]
        $resourceType,
        [Parameter(Mandatory=$true)]
        [String]
        $resourceName
    )

    if ($resourceType -eq [ResourceType]::StorageAccount)
    {
        return "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$resourceName"
    }
    elseif ($resourceType -eq [ResourceType]::AppConfig)
    {
        return "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.AppConfiguration/configurationStores/$resourceName"
    }
    else
    {
        throw "Not supported"
    }
}

function AssignRoles {
    param (
        [Parameter(Mandatory=$true)]
        [ResourceType]
        $resourceType,
        [Parameter(Mandatory=$true)]
        [PSCustomObject]$resource
    )

    $private:scope = GetScope $resourceType $($resource.name)

    foreach ($sp in $resource.servicePrincipals)
    {
        Write-Host "Configuring $($sp.displayName) in $($resource.name)"
        $private:objectId = (Get-AzADServicePrincipal -DisplayName $($sp.displayName)).Id

        if ($null -eq $objectId)
        {
            Write-Error "Cannot find $($sp.displayName)"
            continue
        }

        $private:currentRoles = Get-AzRoleAssignment -ObjectId $objectId -Scope $scope
        foreach ($role in $sp.roles)
        {
            $private:exists = $currentRoles | Where-Object { $_.RoleDefinitionName -eq $role }
            if ($null -eq $exists)
            {
                New-AzRoleAssignment -ObjectID $objectId -RoleDefinitionName $role -Scope $scope
            }
            else
            {
                if ($showWarnings)
                {
                    Write-Warning "$($sp.displayName) already has '$role' assigned to $($resource.name)"
                }
            }
        }
    }
}

Set-AzContext -SubscriptionId $subscriptionId | Out-Null
$local:data = Get-Content $rolesFile -Raw | ConvertFrom-Json

foreach ($storageAccount in $data.storageAccounts)
{
    AssignRoles StorageAccount $storageAccount
}

foreach ($appConfig in $data.appConfigs)
{
    AssignRoles AppConfig $appConfig
}
