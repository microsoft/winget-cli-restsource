---
external help file: Microsoft.WinGet.RestSource-help.xml
Module Name: microsoft.WinGet.RestSource
online version:
schema: 2.0.0
---

# Find-WinGetManifest

## SYNOPSIS
Connects to the specified Windows Package Manager source REST API to retrieve available Manifests, returning only the package identifier, name, publisher and versions for each Manifest result.

## SYNTAX

```
Find-WinGetManifest [-FunctionName] <String> [-Query <String>] [-PackageIdentifier <String>]
 [-PackageName <String>] [-SubscriptionName <String>] [-Exact] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION
Connects to the specified Windows Package Manager source REST API to retrieve available Manifests, returning only the package identifier, name, publisher and versions for each Manifest result.  
This function does not return the full WinGetManifest since the results may be very large.
Use Get-WinGetManifest for retrieving individual full WinGetManifest.

## EXAMPLES

### EXAMPLE 1
```
Find-WinGetManifest -FunctionName "contosorestsource" -Query "PowerToys"
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to search for manifests with PowerToys.

### EXAMPLE 2
```
Find-WinGetManifest -FunctionName "contosorestsource" -Query "PowerToys" -PackageIdentifier "Windows.PowerToys"
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to search for manifests with PowerToys and filter the result with package identifier Windows.PowerToys.

### EXAMPLE 3
```
Find-WinGetManifest -FunctionName "contosorestsource" -Query "PowerToys" -PackageName "Windows PowerToys" -Exact
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to search for manifests with PowerToys and filter the result with package name Windows PowerToys.
Use exact match.

## PARAMETERS

### -FunctionName
Name of the Azure Function that contains the Windows Package Manager REST source.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Query
\[Optional\] The query to be performed against Windows Package Manager REST source.
Empty query will return all manifest infos.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PackageIdentifier
\[Optional\] Filter the search results with PackageIdentifier.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PackageName
\[Optional\] Filter the search results with PackageName.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] The name of the subscription containing the Windows Package Manager REST source.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Exact
\[Optional\] If specified, the Windows Package Manager REST source search will be performed with exact match.

```yaml
Type: SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: False
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

## NOTES

## RELATED LINKS
