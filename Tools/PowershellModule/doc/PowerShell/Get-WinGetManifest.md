---
external help file: Microsoft.WinGet.RestSource-help.xml
Module Name: microsoft.WinGet.RestSource
online version:
schema: 2.0.0
---

# Get-WinGetManifest

## SYNOPSIS
Connects to the specified Windows Package Manager source, or local file system path to retrieve the package manifest, returning
the manifest found.

## SYNTAX

### Azure (Default)
```
Get-WinGetManifest [-FunctionName] <String> [-PackageIdentifier] <String> [-SubscriptionName <String>]
 [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

### File
```
Get-WinGetManifest [-Path] <String> [-PriorManifest <WinGetManifest>] [-ProgressAction <ActionPreference>]
 [<CommonParameters>]
```

## DESCRIPTION
Connects to the specified Windows Package Manager source, or local file system path to retrieve the package Manifest, returning
the manifest found.  
Allows for retrieving results based on the package identifier when targeting the Windows Package Manager source.

## EXAMPLES

### EXAMPLE 1
```
Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys"
```

Returns a Manifest object based on the files found within the specified Path.

### EXAMPLE 2
```
Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"
```

Returns a Manifest object (*.json) of the specified JSON file.

### EXAMPLE 3
```
Get-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys"
```

Returns a Manifest object of the specified Package Identifier that is queried against the Windows Package Manager REST source.

### EXAMPLE 4
```
Get-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"
```

Returns a Manifest object of the specified Package Identifier that is queried against the Windows Package Manager REST source from the specified Subscription Name.

## PARAMETERS

### -Path
Points to either a folder containing a specific application's manifest of type .json or .yaml or to a specific .json or .yaml file.

If you are processing a multi-file manifest, point to the folder that contains all yamls.
Note: all yamls within the folder must be part of
the same package manifest.

```yaml
Type: String
Parameter Sets: File
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PriorManifest
A WinGetManifest object containing a single Windows Package Manager REST source Manifest that will be merged with locally processed .yaml files.
This is used by the script infrastructure internally.

```yaml
Type: WinGetManifest
Parameter Sets: File
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FunctionName
Name of the Azure Function Name that contains the Windows Package Manager REST source.

```yaml
Type: String
Parameter Sets: Azure
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PackageIdentifier
Supports input from pipeline.
The Windows Package Manager Package Identifier of a specific Package Manifest result.

```yaml
Type: String
Parameter Sets: Azure
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] Name of the Azure Subscription that contains the Azure Function which contains the Windows Package Manager REST source.

```yaml
Type: String
Parameter Sets: Azure
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

## NOTES

## RELATED LINKS
