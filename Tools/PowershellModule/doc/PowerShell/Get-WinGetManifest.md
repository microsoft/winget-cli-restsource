---
external help file: Microsoft.WinGet.Source-help.xml
Module Name: Microsoft.WinGet.Source
online version:
schema: 2.0.0
---

# Get-WinGetManifest

## SYNOPSIS
Connects to the specified source REST API, or local file system path to retrieve the application Manifests, returning an array of all Manifests found.
Allows for filtering results based on the name when targetting the REST APIs.

## SYNTAX

### Azure (Default)
```
Get-WinGetManifest [-FunctionName] <String> [[-ManifestIdentifier] <String>] [[-SubscriptionName] <String>]
 [<CommonParameters>]
```

### File
```
Get-WinGetManifest [-Path] <String> [<CommonParameters>]
```

## DESCRIPTION
Connects to the specified source REST API, or local file system path to retrieve the application Manifests, returning an array of all Manifests found.
Allows for filtering results based on the name.
    
The following Azure Modules are used by this script:
    Az.Resources
    Az.Accounts
    Az.Websites
    Az.Functions

## EXAMPLES

### EXAMPLE 1
```
Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys"
```

Returns an array of the Application Manifest objects based on the files (*.yaml or *.json) found within the specified Path.

### EXAMPLE 2
```
Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"
```

Returns an Application Manifest object (*.json) of the specified JSON file.

### EXAMPLE 3
```
Get-WinGetManifest -FunctionName "contosorestsource" -ManifestIdentifier "Windows.PowerToys"
```

Returns an Manifest object of the specified Application Package Identifier that is queried against in the REST APIs.

### EXAMPLE 4
```
Get-WinGetManifest -FunctionName "contosorestsource" -ManifestIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"
```

Returns an Application Manifest object of the specified Package Identifier that is queried against in the REST APIs from the specified Subscription Name.

### EXAMPLE 5
```
Get-WinGetManifest -FunctionName "contosorestsource"
```

Returns an array of Application Manifest objects that are found in the specified Azure Function.

## PARAMETERS

### -Path
Path to a file (*.json) or folder containing *.yaml or *.json files.

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

### -FunctionName
Name of the Azure Function Name that contains the Windows Package Manager REST APIs.

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

### -ManifestIdentifier
\[Optional\] The Windows Package Manager Package Identifier of a specific Manifest result

```yaml
Type: String
Parameter Sets: Azure
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] Name of the Azure Subscription that contains the Azure Function which contains the REST APIs.

```yaml
Type: String
Parameter Sets: Azure
Aliases:

Required: False
Position: 3
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
