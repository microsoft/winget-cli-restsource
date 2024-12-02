# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function New-MicrosoftEntraIdApp
{
    <#
    .SYNOPSIS
    Creates a new Microsoft Entra Id app registration to be used for WinGet rest source Microsoft Entra Id based authentication.

    .DESCRIPTION
    Creates a new Microsoft Entra Id app registration to be used for WinGet rest source Microsoft Entra Id based authentication.
    Always create a new Microsoft Entra Id app registration.
    
    .PARAMETER Name
    Name of the Microsoft Entra Id app to be created.

    .EXAMPLE
    New-MicrosoftEntraIdApp -Name "contosoapp"

    Assumes an active connection to Azure. Creates a new Microsoft Entra Id app named "contosoapp".
    #>

    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string]$Name
    )
    
    $Return = @{
        Result = $false
        Resource = ""
        ResourceScope = ""
    }
    
    ## Normalize Microsoft Entra Id app name
    $NormalizedName = $Name -replace "[^a-zA-Z0-9-()_.]", ""
    if($Name -cne $NormalizedName) {
        $Name = $NormalizedName
        Write-Warning "Removed special characters from the Microsoft Entra Id app name (New Name: $Name)."
    }
    
    ## Creating a line break from previous steps
    Write-Information "Microsoft Entra Id app name to be created: $Name"

    $App = New-AzADApplication -DisplayName $Name -AvailableToOtherTenants $false
    if (!$App)
    {
        Write-Error "Failed to create Microsoft Entra Id app. Name: $Name"
        return $Return
    }

    ## Add App Id Uri
    $AppId = $App.AppId
    Update-AzADApplication -ApplicationId $AppId -IdentifierUri "api://$AppId" -ErrorVariable ErrorUpdate
    if ($ErrorUpdate)
    {
        Write-Error "Failed to add App Id Uri"
        return $Return
    }

    ## Add Api scope
    $ScopeId = [guid]::NewGuid().ToString()
    $ScopeName = "user_impersonation"
    $Api = @{
        Oauth2PermissionScopes = @(
            @{
                AdminConsentDescription = "Sign in to access $Name WinGet rest source"
                AdminConsentDisplayName = "Access WinGet rest source"
                UserConsentDescription  = "Sign in to access $Name WinGet rest source"
                UserConsentDisplayName  = "Access WinGet rest source"
                Id = $ScopeId
                IsEnabled = $true
                Type = "User"
                Value = $ScopeName
            }
        )
    }
    Update-AzADApplication -ApplicationId $AppId -Api $Api -ErrorVariable ErrorUpdate
    if ($ErrorUpdate)
    {
        Write-Error "Failed to add Api scope"
        return $Return
    }

    ## Add authorized client
    $Api = @{
        PreAuthorizedApplications = @(
            @{
                AppId = "7b8ea11a-7f45-4b3a-ab51-794d5863af15"
                DelegatedPermissionIds = @($ScopeId)
            }
            @{
                AppId = "04b07795-8ddb-461a-bbee-02f9e1bf7b46"
                DelegatedPermissionIds = @($ScopeId)
            }
            @{
                AppId = "1950a258-227b-4e31-a9cf-717495945fc2"
                DelegatedPermissionIds = @($ScopeId)
            }
        )
    }
    Update-AzADApplication -ApplicationId $AppId -Api $Api -ErrorVariable ErrorUpdate
    if ($ErrorUpdate)
    {
        Write-Error "Failed to add authorized clients"
        return $Return
    }

    $Return.Result = $true
    $Return.Resource = $AppId
    $Return.ResourceScope = $ScopeName
    return $Return
}