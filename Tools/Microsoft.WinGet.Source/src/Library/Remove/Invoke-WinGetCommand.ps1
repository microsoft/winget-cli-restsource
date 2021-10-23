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

class WinGetPackage
{
    [string]$Name
    [string]$Id
    [string]$Version
    [string]$Available
    [string]$Source

    WinGetPackage ([string] $a, [string]$b, [string]$c, [string]$d, [string]$e)
    {
        $this.Name    = $a.TrimEnd()
        $this.Id      = $b.TrimEnd()
        $this.Version = $c.TrimEnd()
        $this.Available = $d.TrimEnd()
        $this.Source  = $e.TrimEnd()
    }
    
    WinGetPackage ([WinGetPackage] $a) {
        $this.Name    = $a.Name
        $this.Id      = $a.Id
        $this.Version = $a.Version
        $this.Available = $a.Available
        $this.Source  = $a.Source

    }
    WinGetPackage ([psobject] $a) {
        $this.Name    = $a.Name
        $this.Id      = $a.Id
        $this.Version = $a.Version
        $this.Available = $a.Available
        $this.Source  = $a.Source
    }
    
    WinGetSource ([string[]]$a)
    {
        $this.name     = $a[0].TrimEnd()
        $this.Id       = $a[1].TrimEnd()
        $this.Version  = $a[2].TrimEnd()
        $this.Available = $a[3].TrimEnd()
        $this.Source   = $a[4].TrimEnd()
    }

    
    [WinGetPackage[]] Add ([WinGetPackage] $a)
    {
        $FirstValue  = [WinGetPackage]::New($this)
        $SecondValue = [WinGetPackage]::New($a)

        [WinGetPackage[]]$Result = @([WinGetPackage]::New($FirstValue), [WinGetPackage]::New($SecondValue))

        Return $Result
    }

    [WinGetPackage[]] Add ([String[]]$a)
    {
        $FirstValue  = [WinGetPackage]::New($this)
        $SecondValue = [WinGetPackage]::New($a)
        
        [WinGetPackage[]] $Combined = @([WinGetPackage]::New($FirstValue), [WinGetPackage]::New($SecondValue))

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

        switch ($b) {
            "json" 
                { $this.Manifest = $($($($($($c -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}") -replace "`t|`n|`r|  ","") }
            "yaml"
                {  }
            Default 
                { $this.Manifest = $c }
        }
    }

    WinGetManifestFile ([string[]]$a)
    {
        $this.Path      = $a[0]
        $this.Type      = $a[1]
        $this.Manifest  = $a[2]

        switch ($a[1])
        {
            "json" 
                { $this.Manifest = $($($($($($a[2] -replace ": ",":") -replace " { |{ ", "{") -replace ', ', ',') -replace " } | }", "}") -replace "`t|`n|`r|  ","") }
            "yaml"
                {  }
            Default 
                { $this.Manifest = $a[2] }
        }
    }
    
    WinGetManifestFile([WinGetManifestFile]$a)
    {
        $this.Path     = $a.Path
        $this.Type     = $a.Type
        $this.Manifest = $a.Manifest
    }
}

Function Invoke-WinGetCommand
{
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [string[]]  $WinGetArgs,
        [Parameter(Position=0, Mandatory=$true)] [string[]]$IndexTitles
    )
    BEGIN
    {
        $Index  = @()
        $Result = @()
        $i      = 0
        $IndexTitlesCount = $IndexTitles.Count
        
        ## Remove two characters from the string length and add "..." to the end (only if there is the three below characters present).
        [string[]]$WinGetSourceListRaw = & "WinGet" $WingetArgs | out-string -stream | foreach-object{$_ -replace ("$([char]915)$([char]199)$([char]170)", "$([char]199)")}
        
    }
    PROCESS
    {
        ## Gets the indexing of each title
        $Offset = 0
        while ($WinGetSourceListRaw[$Offset].Split(" ")[0].Trim() -ne $IndexTitles[0]){$Offset++}
        foreach ($IndexTitle in $IndexTitles) {
            ## Creates an array of titles and their string location
            $IndexStart = $WinGetSourceListRaw[$Offset].IndexOf($IndexTitle)
            $IndexEnds  = ""

            IF($IndexStart -ne "-1") {
                $Index += [pscustomobject]@{
                    Title = $IndexTitle
                    Start = $IndexStart
                    Ends = $IndexEnds
                    }
            }
        }

        ## Orders the Object based on Index value
        $Index = $Index | Sort-Object Start

        ## Sets the end of string value
        while ($i -lt $IndexTitlesCount) {
            $i ++

            ## Sets the End of string value (if not null)
            if($Index[$i].Start) {
                $Index[$i-1].Ends = ($Index[$i].Start -1) - $Index[$i-1].Start 
            }
        }

        ## Builds the WinGetSource Object with contents
        $i = $Offset + 2
        while($i -lt $WinGetSourceListRaw.Length) {
            $row = $WinGetSourceListRaw[$i]
            try {
                [bool] $TestNotTitles     = $WinGetSourceListRaw[0] -ne $row
                [bool] $TestNotHyphenLine = $WinGetSourceListRaw[1] -ne $row -and !$Row.Contains("---")
                [bool] $TestNotNoResults  = $row -ne "No package found matching input criteria."
            }
            catch {Wait-Debugger}

            if(!$TestNotNoResults) {
                Write-LogEntry -LogEntry "No package found matching input criteria." -Severity 1
            }

            ## If this is the first pass containing titles or the table line, skip.
            if($TestNotTitles -and $TestNotHyphenLine -and $TestNotNoResults) {
                $List = @{}

                foreach($item in $Index) {
                    if($Item.Ends) {
                        #try {
                            $List[$Item.Title] = $row.SubString($item.Start,$Item.Ends)
                        #}
                        #catch {
                        #    Wait-Debugger
                        #}
                        #Wait-Debugger
                        
                    }
                    else {
                        $List[$item.Title] = $row.SubString($item.Start, $row.Length - $Item.Start)
                    }
                }

                $result += [pscustomobject]$list
            }
            $i++
        }
    }
    END
    {
        return $Result
    }
}

