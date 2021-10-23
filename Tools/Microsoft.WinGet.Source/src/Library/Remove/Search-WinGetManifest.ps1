
Function Search-WinGetManifest
{
    PARAM(
        [Parameter(Position=0, Mandatory=$false)]  [string]$ManifestName,
        [Parameter(Position=0, Mandatory=$false)] [string]$Source

    )
    BEGIN
    {
        ## If a Source is provided, then append "-s Source" to the query
        if($Source) {
            [string] $InvokeExpression = "WinGet Search $ManifestName -s $Source"
        }
        else {
            [string] $InvokeExpression = "WinGet Search $ManifestName"
        }
        
        [WinGetManifest[]]$Result = @()

        ## Indexing of titles
        $IndexTitles      = @("Name", "Id", "Version", "Source")
    }
    PROCESS
    {
        $List = Invoke-WinGetCommand -InvokeExpression $InvokeExpression -IndexTitles $IndexTitles

        foreach ($Obj in $List) {
            $Result += [WinGetManifest]::New($Obj.Name, $Obj.Id, $Obj.Version, $Obj.Source)
        }
    }
    END
    {
        return $Result
    }
}

