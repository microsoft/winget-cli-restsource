<#
Created On: 2021-09-22
Created By: Roy MacLachlan

Description:
This library can be dot loaded into memory allowing for access to pre-created functions enabling an
easier Windows Package Manager private source manifest experience. The functions in this file support
Adding a new Manifest to your Windows Package Manager private source in Azure.

After dot loading this file, it'll complete a validation check to ensure you have the required Azure
manifests installed. If any manifest is missing, instructions on how to install will be displayed to
the screen.

Example:
To dot load this file into memory:
. .\PrivateRepoLib.ps1
#>

class ARMObject
{
    [ValidateSet("AppInsight", "Keyvault", "StorageAccount", "asp", "CosmosDBAccount", "CosmosDBDatabase", "CosmosDBContainer", "Function", "FrontDoor")]
    [ValidateNotNullOrEmpty()][string] $ObjectType
    [ValidateNotNullOrEmpty()][string] $ParameterPath
    [ValidateNotNullOrEmpty()][string] $TemplatePath
    $Parameters = @{
        '$Schema' = "1.0.0.0"
        contentVersion = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#"
        Parameters = @{}
    }

    ARMObject ([System.Collections.Hashtable]$Var)
    {
        $this.ObjectType    = $Var.ObjectType
        $this.ParameterPath = $Var.ParameterPath
        $this.TemplatePath  = $Var.TemplatePath
        $this.Parameters.contentVersion = $Var.TemplatePath.contentVersion
        $this.Parameters.Parameters     = $Var.TemplatePath.Parameters
        IF($null -ne $Var.TemplatePath.'$Schema')
            { $this.Parameters.'$Schema'      = $Var.TemplatePath.'$Schema' }
        IF($null -ne $Var.TemplatePath.contentVersion)
            { $this.Parameters.contentVersion = $Var.TemplatePath.contentVersion }
        IF($null -ne $Var.TemplatePath.Parameters)
            { $this.Parameters.Parameters     = $Var.TemplatePath.Parameters }
    }
    ARMObject ([string] $a, [string] $b, [string] $c)
    {
        $this.ObjectType    = $a
        $this.ParameterPath = $b
        $this.TemplatePath  = $c
    }
    ARMObject ([string] $a, [string] $b, [string] $c, $d)
    {
        $this.ObjectType    = $a
        $this.ParameterPath = $b
        $this.TemplatePath  = $c
        $this.Parameters    = $d
    }
    [boolean]TestParameterPath()
    {
        Return Test-Path -Path $this.ParameterPath
    }
    [boolean]TestTemplatePath()
    {
        Return Test-Path -Path $this.TemplatePath
    }
}

class WinGetSource
{
    [string] $Name
    [string] $Argument

    WinGetSource ()
    {  }

    WinGetSource ([string]$a, [string]$b)
    {
        $this.Name     = $a.TrimEnd()
        $this.Argument = $b.TrimEnd()
    }

    WinGetSource ([string[]]$a)
    {
        $this.name     = $a[0].TrimEnd()
        $this.Argument = $a[1].TrimEnd()
    }
    
    WinGetSource ([WinGetSource]$a)
    {
        $this.Name     = $a.Name.TrimEnd()
        $this.Argument = $a.Argument.TrimEnd()
    }
    
    [WinGetSource[]] Add ([WinGetSource]$a)
    {
        $FirstValue  = [WinGetSource]::New($this)
        $SecondValue = [WinGetSource]::New($a)
        
        [WinGetSource[]] $Combined = @([WinGetSource]::New($FirstValue), [WinGetSource]::New($SecondValue))

        Return $Combined
    }

    [WinGetSource[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetSource]::New($this)
        $SecondValue = [WinGetSource]::New($a)
        
        [WinGetSource[]] $Combined = @([WinGetSource]::New($FirstValue), [WinGetSource]::New($SecondValue))

        Return $Combined
    }
}

Class WinGetManifestFile
{
    [string]$Path
    [string]$Type
    [string]$Manifest

    WinGetManifestFile()
    {}
    
    WinGetManifestFile([string]$a, [string]$b, [string]$c)
    {
        $this.Path     = $a
        $this.Type     = $b
        $this.Manifest = $c
    }

    WinGetManifestFile ([string[]]$a)
    {
        $this.Path      = $a[0]
        $this.Type      = $a[1]
        $this.Manifest  = $a[2]
    }
    
    WinGetManifestFile([WinGetManifestFile]$a)
    {
        $this.Path     = $a.Path
        $this.Type     = $a.Type
        $this.Manifest = $a.Manifest
    }
}

class WinGetManifest
{
    [string]$Name
    [string]$Id
    [string]$Version
    [string]$Source

    WinGetManifest ([string] $a, [string]$b, [string]$c, [string]$d)
    {
        $this.Name    = $a.TrimEnd()
        $this.Id      = $b.TrimEnd()
        $this.Version = $c.TrimEnd()
        $this.Source  = $d.TrimEnd()
    }
    
    WinGetManifest ([WinGetManifest] $a)
    {
        $this.Name    = $a.Name
        $this.Id      = $a.Id
        $this.Version = $a.Version
        $this.Source  = $a.Source
    }
    
    WinGetSource ([string[]]$a)
    {
        $this.name     = $a[0].TrimEnd()
        $this.Id       = $a[1].TrimEnd()
        $this.Version  = $a[2].TrimEnd()
        $this.Source   = $a[3].TrimEnd()
    }

    
    [WinGetManifest[]] Add ([WinGetManifest] $a)
    {
        $FirstValue  = [WinGetManifest]::New($this)
        $SecondValue = [WinGetManifest]::New($a)

        [WinGetManifest[]]$Result = @([WinGetManifest]::New($FirstValue), [WinGetManifest]::New($SecondValue))

        Return $Result
    }

    [WinGetManifest[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetManifest]::New($this)
        $SecondValue = [WinGetManifest]::New($a)
        
        [WinGetManifest[]] $Combined = @([WinGetManifest]::New($FirstValue), [WinGetManifest]::New($SecondValue))

        Return $Combined
    }
}

Function Run-WinGetCommand
{
    Param(
        [Parameter(Position=0, Mandatory=$true)] [string]  $InvokeExpression,
        [Parameter(Position=0, Mandatory=$true)] [string[]]$IndexTitles
    )
    Begin
    {
        $Index = @()
        $i     = 0
        $IndexTitlesCount = $IndexTitles.Count
        $Result = @()

        [string[]]$WinGetSourceListRaw    = Invoke-Expression -Command $InvokeExpression
        
    }
    Process
    {
        ## Gets the indexing of each title
        foreach ($IndexTitle in $IndexTitles) 
        {
            ## Creates an array of titles and their string location
            $Obj = New-Object PSObject
            $IndexStart = $WinGetSourceListRaw[0].IndexOf($IndexTitle)

            ## Some WinGet Results return an empty first line.
            IF($IndexStart -eq "-1")
                { $IndexStart = $WinGetSourceListRaw[1].IndexOf($IndexTitle) }

            $IndexEnds  = ""
            
            Add-Member -InputObject $Obj -MemberType NoteProperty -Name Title -Value $IndexTitle
            Add-Member -InputObject $Obj -MemberType NoteProperty -Name Start -Value $IndexStart
            Add-Member -InputObject $Obj -MemberType NoteProperty -Name Ends  -Value $IndexEnds

            $Index += $Obj
        }

        ## Orders the Object based on Index value
        $Index = $($Index | Sort-Object Start)

        ## Sets the end of string value
        While ($i -lt $IndexTitlesCount)
        {
            $i += 1

            ## Sets the End of string value (if not null)
            IF($Index[$i].Start)
                { $Index[$i-1].Ends = ($Index[$i].Start -1) - $Index[$i-1].Start }
        }

        ## Builds the WinGetSource Object with contents
        foreach ($row in $WinGetSourceListRaw)
        {
            ## If this is the first pass containing titles or the table line, skip.
            If($WinGetSourceListRaw[0] -ne $row -and $WinGetSourceListRaw[1] -ne $row -and !$Row.Contains("---"))
            {
                $List = New-Object PSObject
                [int]     $RowLength    = $Row.Length

                Foreach ($item in $Index)
                {
                    IF($Item.Ends)
                        { Add-Member -InputObject $List -MemberType NoteProperty -Name $Item.Title -Value $row.Substring($item.Start, $Item.Ends) }
                    Else
                        { Add-Member -InputObject $List -MemberType NoteProperty -Name $Item.Title -Value $row.Substring($item.Start, $RowLength - $item.Start) }
                }

                $Result += $List
            }
        }
    }
    End
    {
        Return $Result
    }
}

Function Get-WinGetSource
{
    Begin
    {
        [string]        $InvokeExpression = "WinGet Source List"
        [WinGetSource[]]$Result = @()

        ## Indexing of titles
        $IndexTitles      = @("Name", "Argument")
    }
    Process
    {
        $List = Run-WinGetCommand -InvokeExpression $InvokeExpression -IndexTitles $IndexTitles

        Foreach ($Obj in $List)
            { $Result += [WinGetSource]::New($Obj.Name, $Obj.Argument) }
    }
    End
    {
        Return $Result
    }
}

Function Search-WinGetManifests
{
    Param(
        [Parameter(Position=0, Mandatory=$true)] [string]$ManifestName
    )
    Begin
    {
        [string]        $InvokeExpression = "WinGet Search $ManifestName"
        [WinGetManifest[]]$Result = @()

        ## Indexing of titles
        $IndexTitles      = @("Name", "Id", "Version", "Source")
    }
    Process
    {
        $List = Run-WinGetCommand -InvokeExpression $InvokeExpression -IndexTitles $IndexTitles

        Foreach ($Obj in $List)
            { $Result += [WinGetManifest]::New($Obj.Name, $Obj.Id, $Obj.Version, $Obj.Source) }
    }
    End
    {
        Return $Result
    }
}

Function Write-LogEntry
{
    Param(
        [Parameter(Position=0, Mandatory=$true)]  [string] $LogEntry,
        [Parameter(Position=1, Mandatory=$false)] [int]    $Severity=1,
        [Parameter(Position=2, Mandatory=$false)] [string] $FontColor="",
        [Parameter(Position=3, Mandatory=$false)] [int]    $Indent = 0,
        [Parameter(Position=4, Mandatory=$false)] [switch] $NoNewLine
    )
    Begin
    {
        If($FontColor -eq "")
        {
            switch ($Severity) {
                "1" 
                {
                    ## Informational Response
                    $FontColor     = "White"
                    $MessagePreFix = ""
                }
                "2" 
                {
                    ## Warning Response
                    $FontColor = "Yellow"
                    $MessagePreFix = "WARNING:  "
                }
                "3" 
                {
                    ## Error Response
                    $FontColor = "Red"
                    $MessagePreFix = "ERROR:    "
                }
            }
        }
        ## Combines the logging message and the message type as a prefix
        $LogEntry = $MessagePreFix + $LogEntry

        ## Indents the message when viewed on the screen.
        $LogIndent = "  "
        While ($Indent -gt 0)
        {
            $LogEntry = $LogIndent + $LogEntry
            $Indent -= 1
        }
    }
    Process
    {
        ## Writes logging to the screen
        If($NoNewLine)
            { Write-Host -Object $LogEntry -ForegroundColor $FontColor -NoNewline }
        else 
            { Write-Host -Object $LogEntry -ForegroundColor $FontColor }
    }
    End
    {
        Return
    }
}

Function Test-RequiredModules
{
    [CmdletBinding(DefaultParameterSetName = 'Multiple')]
    Param(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Single")] [string]$RequiredModule,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Multiple")] [string[]]$RequiredModules
    )
    Begin
    { 
        ## Validation result to be returned is True until proven otherwise.
        $ValidationStatus = $true
    }
    Process
    {
        switch ($PsCmdlet.ParameterSetName) {
            "Multiple" 
            {
                ## If an array of required Modules is provided, cycle through each individually.
                Foreach ($RequiredModule in $RequiredModules) 
                {
                    ## Tests if the module is installed
                    $Result = Test-RequiredModules -RequiredModule $RequiredModule
                    
                    ## Specifies that a module is missing
                    IF(!($Result))
                        { $ValidationStatus = $false }
                }

                IF(!($ValidationStatus))
                {
                    ## Module Validation failed
                    $ErrorMessage = "Missing required PowerShell modules`n"
                    $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az`n`n"
                    
                    Write-LogEntry $ErrorMessage -Severity 3
                }
            }
            "Single" 
            { 
                ## Determines if the PowerShell Module is missing, If missing false if missing
                IF(!$(Get-Module -ListAvailable -Name $RequiredModule) )
                    { $ValidationStatus = $false } 
            }
        }
    }
    End
    {
        ## Returns a value only if the module is missing
        Return $ValidationStatus
    }
}

Function Test-ConnectionToAzure
{
    Begin
    {
        $AzContext = Get-AzContext
    }
    Process
    {
        IF($null -eq $AzContext)
            { 
                Write-LogEntry -LogEntry "Not connected to Azure, please connect to your Azure Subscription" -Severity 1
                $Result = $false 
            }
        Else
            { 
                Write-LogEntry -LogEntry "Connected to Azure" -Severity 1
                $Result = $true 
            }
    }
    End
    {
        Return $Result
    }
}

Function Test-AzureResource
{
    Param(
        [Parameter(Position=0, Mandatory = $false)] [string]$AzureResourceGroupName="",
        [Parameter(Position=1, Mandatory = $false)] [string]$AzureFunctionName=""
    )
    Begin
    {
        $Result = $true
        
        $AzureFunctionNameNotNullOrEmpty = $true
        $AzureFunctionNameExists         = $true

        $AzureResourceGroupNameNotNullOrEmpty = $true
        $AzureResourceGroupNameExists         = $true

        ## Determines if the Azure Function App Name is not null or empty
        If($AzureFunctionName.Length -le 0)
        { 
            $AzureFunctionName               = "<null>"
            $AzureFunctionNameNotNullOrEmpty = $false 
        }
    
        ## Determines if the Azure Function App Name is in Azure
        If($(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).Count -le 0)
            { $AzureFunctionNameExists = $false }
        
        ##Determines if the Azure Resource Group Name is not null or empty
        If($AzureResourceGroupName.Length -le 0)
        {   
            $AzureResourceGroupName               = "<null>"
            $AzureResourceGroupNameNotNullOrEmpty = $false 
        }

        If($(Get-AzResourceGroup).Where({$_.ResourceGroupName -eq $AzureResourceGroupName}).Count -lt 0)
            { $AzureResourceGroupNameExists = $false }

   }
    Process
    {
        ## If there is an error with the value, it will be shown as "Red". If no issues then display the text as green.
        Write-LogEntry -LogEntry "`nAzure Resources:" -Severity 1
        Write-LogEntry -LogEntry "  Azure Function:       " -NoNewline; If($AzureFunctionNameExists)     { Write-LogEntry $AzureFunctionName      -FontColor Green }Else{ Write-LogEntry -LogEntry "$AzureFunctionName"      -FontColor Red }
        Write-LogEntry -LogEntry "  Azure Resource Group: " -NoNewline; If($AzureResourceGroupNameExists){ Write-LogEntry $AzureResourceGroupName -FontColor Green }Else{ Write-LogEntry -LogEntry "$AzureResourceGroupName" -FontColor Red }

        ## If either the Azure Function Name or the Azure Resource Group Name are null, error.
        IF(!$AzureFunctionNameNotNullOrEmpty -or !$AzureResourceGroupNameNotNullOrEmpty -or !$AzureFunctionNameExists -or !$AzureResourceGroupNameExists)
        {
            Write-Host ""
            Write-LogEntry -LogEntry "Both the Azure Function and Resource Group Names can not be null and must exist. Please verify that the Azure function and Resource Group exist.`n" -Severity 3
            $Result = $false
        }

    }
    End
    {
        Return $Result
    }
}

Function Connect-ToAzure
{
    Param(
        [Parameter(Position=0, Mandatory=$false)] [string]$AzureSubscriptionName
    )
    Begin
    {
        $Result = $true
        $AzConnected = Test-ConnectionToAzure
    }
    Process
    {
        If(!($AzConnected))
        {
            ## No active connections to Azure
            Write-LogEntry -LogEntry "Not connected to Azure, please connect to your Azure Subscription" -Severity 1
            
            ## Determines that a connection to Azure is neccessary, and if a Subscription Name was provided, connect to that Subscription
            If($AzureSubscriptionName -eq "")
                { Connect-AzAccount }
            else 
                { Connect-AzAccount -SubscriptionName $AzureSubscriptionName }

            ## If the connection fails, or the user cancels the login request, then throw an error.
            $AzConnected = Test-ConnectionToAzure
            If($null -eq $Result)
            { 
                Write-LogEntry "Failed to connect to Azure" -Severity 3
                $Result = $false
            }
        }
    }
    End
    {
        Return $Result
    }
}

Function Get-WinGetManifestFile
{
    Param(
        [Parameter(Position=0, Mandatory=$true)] [string]$ManifestFilePath
    )
    Begin
    {
        Write-LogEntry -LogEntry "Retrieving the Application Manifest" -Severity 1

        $Result              = $true
        $ManifestFile        = ""
        $ManifestFileExists  = Test-Path -Path $ManifestFilePath -PathType Leaf

        IF($ManifestFileExists)
        { 
            ## Gets the Manifest object and contents of the Manifest - identifying the manifest file extention.
            $ApplicationManifest = Get-Content -Path $ManifestFilePath -Raw
            $ManifestFile        = Get-Item -Path $ManifestFilePath
            $ManifestFileType    = $ManifestFile.Extension
        }
        else 
        {
            ## The Manifest path did not resolve to a file.
            $Result = $false
            Write-LogEntry -LogEntry "Unable to locate the Manifest File, verify the file exists and try again." -Severity 3
        }
    }
    Process
    {
        switch ($ManifestFileType) 
        {
            ## If the File type is JSON
            ".json" 
            {
                ## Removes spacing from the JSON content
                $ApplicationManifest = $($ApplicationManifest -replace "`t|`n|`r|  ","")
                $ApplicationManifest = $($($($($ApplicationManifest -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")

                $Result = Test-WinGetManifest -Manifest $ApplicationManifest
                IF($Result)
                {
                    ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                    $Result = $ApplicationManifest
                }
            }
            ## If the File type is YAML
            ".yaml"
            {

                $Result = Test-WinGetManifest -Manifest $ApplicationManifest
                IF($Result)
                {
                    ## Sets the return result to be the contents of the JSON file if the Manifest test passed.
                    $Result = $ApplicationManifest
                }
            }
            Default
            {
                If($ManifestFileExists)
                {
                    $Result = $false
                    Write-LogEntry -LogEntry "Incorrect filetype. Verify the file if of type '*.yaml' or '*.json' and try again." -Severity 3
                }
            }
        }
    }
    End
    {
        Return $Result
    }
}

Function Add-WinGetManifestFile
{
    <#
    .SYNOPSIS
    Submits Manifest files to the Azure private source
    
    .DESCRIPTION
    By running this function with the required inputs, it will connect to the Azure Tennant that hosts the Windows Package Manager private source, then collects the required URL for Manifest submission before retrieving the contents of the Manifest JSON to submit.
        
    The following Azure Modules are used by this script:
        Az.Resources --> Invoke-AzResourceAction
        Az.Accounts  --> Connect-AzAccount, Get-AzContext
        Az.Websites  --> Get-AzWebapp
        Az.Functions --> Get-AzFunctionApp
    
    .PARAMETER PrivateRepoName
    Name of the Windows Package Manager private source. Can be identified by running: "winget source list" and using the Repository Name
    
    .PARAMETER AzureFunctionName
    Name of the Azure Function that hosts the private source
    
    .PARAMETER ManifestFilePath
    Path to the JSON manifest file that will be uploaded to the private source
    
    .PARAMETER AzureSubscriptionName
    [Optional] The Subscription name contains the Windows Package Manager private source
    
    .EXAMPLE
    New-WinGetManifest -PrivateRepoName "Private" -ManifestFilePath "C:\Temp\App.json"

    .EXAMPLE
    New-WinGetManifest -PrivateRepoName "Private" -ManifestFilePath "C:\Temp\App.json" -AzureSubscriptionName "Subscription"
    
    .EXAMPLE
    New-WinGetManifest AzureFunctionName "contoso-function-prod" -ManifestFilePath "C:\Temp\App.json"

    .EXAMPLE
    New-WinGetManifest AzureFunctionName "contoso-function-prod" -ManifestFilePath "C:\Temp\App.json" -AzureSubscriptionName "Subscription"
    #>

    [CmdletBinding(DefaultParameterSetName = 'WinGet')]
    Param(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="WinGet")] [string]$PrivateRepoName,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Azure")]  [string]$AzureFunctionName,
        [Parameter(Position=1, Mandatory=$true)]  [string]$ManifestFilePath,
        [Parameter(Position=2, Mandatory=$false)] [string]$AzureSubscriptionName = ""
    )
    Begin
    {
        ###############################
        ## Validates that the Azure Modules are installed
        $RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
        $Result = Test-RequiredModules -RequiredModules $RequiredModules

        If($Result)
            { Throw "Unable to run script, missing required PowerShell modules" }

        ###############################
        ## Connects to Azure, if not already connected.
        $Result = Connect-ToAzure
        IF(!($Result))
            { Throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials." }

        ###############################
        ## Determines the PowerShell Parameter Set that was used in the call of this Function.
        switch ($PsCmdlet.ParameterSetName) 
        {
            "WinGet" {
                ## Sets variables as if the Windows Package Manager was Private Repo Name was specified.
                $AzureFunctionName      = $(Winget source list -n $PrivateRepoName)[4].split("/")[2].Split(".")[0]
                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).ResourceGroupName
             }
            "Azure"  { 
                ## Sets variables as if the Azure Function Name was provided.
                $AzureResourceGroupName = $(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).ResourceGroupName
             }
        }

        ###############################
        ##  Verify Azure Resources Exist
        $Result = Test-AzureResource -AzureFunctionName $AzureFunctionName -AzureResourceGroupName $AzureResourceGroupName
        IF(!$Result)
            { Throw "Failed to confirm resources exist in Azure. Please verify and try again." }

        ###############################
        ## Gets the JSON Content for posting to Private Repo
        $Result = Get-WinGetManifestFile -ManifestFilePath $ManifestFilePath
        IF(!$Result)
            { Throw "Failed to retrieve a proper manifest. Verify and try again." }

        #############################################
        ##############  Rest api call  ##############

        ## Specifies the Rest api call that will be performed
        $TriggerName      = "ManifestPost"
        $apiContentType   = "application/json"
        $apiMethod        = "Post"

        ## Creates the API Post Header
        $apiHeader = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
        $apiHeader.Add("Accept", 'application/json')
    }
    Process
    {
        ## Retrieves the Azure Function URL used to add new manifests to the private source
        $FunctionApp = Get-AzWebApp -ResourceGroupName $AzureResourceGroupName -Name $AzureFunctionName -ErrorAction SilentlyContinue -ErrorVariable err

        $FunctionAppId = $FunctionApp.Id
        $DefaultHostName = $FunctionApp.DefaultHostName
        $FunctionKey = (Invoke-AzResourceAction -ResourceId "$FunctionAppId/functions/$TriggerName" -Action listkeys -Force).default
        $AzFunctionURL = "https://" + $DefaultHostName + "/api/" + "packageManifests" + "?code=" + $FunctionKey
        
        ## Publishes the Manifest to the Windows Package Manager private source
        $Response = Invoke-RestMethod $AzFunctionURL -Headers $apiHeader -Method $apiMethod -Body $ApplicationManifest -ContentType $apiContentType
    }
    End
    {
        Return $Response
    }

}

Function Get-WinGetManifestFile
{}

Function Remove-WingetManifestFile
{}

Function Test-WinGetManifest
{
    [CmdletBinding(DefaultParameterSetName = 'Path')]
    Param(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Path")]  [string]$ManifestFilePath,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Object")]$Manifest,
        [Parameter(Position=1, Mandatory=$false)]  [switch]$ReturnManifest
    )
    Begin
    {
        Switch ($($PSCmdlet.ParameterSetName))
        {
            "Path"{
                If((Test-Path -Path $ManifestFilePath))
                {
                    ## Retrieves the contents of the Manifest file
                    $Manifest = Get-Content -Path $ManifestFilePath -Raw

                    ## Removes any spacing from the Manifest file
                    $Manifest = $($Manifest -replace "`t|`n|`r|  ","")
                    $Manifest = $($($($($Manifest -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")
                }
            }
            "Object"
            {
                ## Removes any spacing from the Manifest file
                $Manifest = $($Manifest -replace "`t|`n|`r|  ","")
                $Manifest = $($($($($Manifest -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}")
            }
        }

        $ManifestFileTypeJSON = $false
        $ReturnResult = $true
    }
    Process
    {

    }
    End
    {
        ## Determines what will be returned from the Function
        IF($ReturnResult)
        {
            ## Returns the Manifest only if the test passes. If the test fails return False
            IF($ReturnResult)
                { Return $Manifest }
            Else
                { Return $ReturnResult }
        }
        Else
        {
            ## Returns the result of the test. If all test pass, the result is True, otherwise will return False
            Return $ReturnResult
        }
    }
}

## Validates that the required Azure Modules are present when the script is imported.
[string[]]$RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
[Boolean] $TestResult = Test-RequiredModules -RequiredModules $RequiredModules

If($TestResult)
{ 
        ## Modules have been identified as missing
        Write-Host ""
        $ErrorMessage = "There are missing PowerShell modules that must be installed.`n"
        $ErrorMessage += "    Some or all PowerShell functions included in this library will fail.`n"
        $ErrorMessage += "    Run the following command to install the missing modules: Install-Module Az -Force`n`n"
        
        Write-LogEntry -LogEntry $ErrorMessage -Severity 2
}