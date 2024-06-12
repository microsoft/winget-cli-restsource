<#
.SYNOPSIS
    Sets roles to a service principals
.DESCRIPTION
    Sets roles to service principals given a json file with the definitions.
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

Set-AzContext -SubscriptionId $subscriptionId | Out-Null
$local:data = Get-Content $rolesFile -Raw | ConvertFrom-Json

foreach ($storageAccount in $data.storageAccounts)
{
    $private:scope = "/subscriptions/$subscriptionId/resourceGroups/$resourceGroup/providers/Microsoft.Storage/storageAccounts/$($storageAccount.name)"

    foreach ($sp in $storageAccount.servicePrincipals)
    {
        Write-Host "Configuring $($sp.displayName) in $($storageAccount.name)"
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
                    Write-Warning "$($sp.displayName) already has '$role' assigned to $($storageAccount.name)"
                }
            }
        }
    }
}