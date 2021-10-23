
Function Get-WinGetSource
{
    BEGIN
    {
        [string]        $InvokeExpression = "WinGet Source Export"
        [WinGetSource[]]$Result = @()

        ## Indexing of titles
        $IndexTitles      = @("Name", "Argument")
    }
    PROCESS
    {
        $List = Run-WinGetCommand -InvokeExpression $InvokeExpression -IndexTitles $IndexTitles -JSON

        Wait-Debugger
        foreach ($Obj in $List) {
            $Result += [WinGetSource]::New($Obj.Name, $Obj.Arg, $Obj.Data, $Obj.Identifier, $Obj.Type)
        }
    }
    END
    {
        return $Result
    }
}