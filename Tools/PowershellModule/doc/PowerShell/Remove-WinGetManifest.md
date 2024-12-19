---
external help file: Microsoft.WinGet.RestSource-help.xml
Module Name: microsoft.WinGet.RestSource
online version:
schema: 2.0.0
---

# Remove-WinGetManifest

## SYNOPSIS
Removes a Manifest from the Windows Package Manager REST source.

## SYNTAX

```
Remove-WinGetManifest [-FunctionName] <String> [-PackageIdentifier] <String> [[-PackageVersion] <String>]
 [-SubscriptionName <String>] [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION
Removes a Manifest from the Windows Package Manager REST source.  
This function will connect to the Azure Tenant that hosts the Windows Package Manager REST source, removing the 
specified Manifest.

## EXAMPLES

### EXAMPLE 1
```
Remove-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys"
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to remove all versions of the specified Manifest from 
the Windows Package Manager REST source.

### EXAMPLE 2
```
Remove-WinGetManifest -FunctionName "contosorestsource" -PackageIdentifier "Windows.PowerToys" -PackageVersion "1.0.0.0"
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to remove specified version of the specified Manifest from 
the Windows Package Manager REST source.

## PARAMETERS

### -FunctionName
Name of the Azure Function that hosts the Windows Package Manager REST source.

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

### -PackageIdentifier
Supports input from pipeline by property.
The Package Identifier that represents the Manifest to be removed.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -PackageVersion
\[Optional\] Supports input from pipeline by property.
The Package version that represents the Manifest to be removed.
If empty, all versions will be removed.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: None
Accept pipeline input: True (ByPropertyName)
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] The Subscription name that contains the Windows Package Manager REST source.

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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

## NOTES

## RELATED LINKS
