---
external help file: Microsoft.WinGet.Source-help.xml
Module Name: Microsoft.WinGet.Source
online version:
schema: 2.0.0
---

# Get-WinGetManifest

## SYNOPSIS
Connects to the specified source Rest API, or local file system path to retrieve the application Manifests, returning an array of all Manifests found.
Allows for filtering results based on the name when targetting the Rest APIs.

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

### Custom
```
Get-WinGetManifest [-URL] <String> [<CommonParameters>]
```

## DESCRIPTION
Connects to the specified source Rest API, or local file system path to retrieve the application Manifests, returning an array of all Manifests found.
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

Returns an array of all Manifest objects based on the files found within the specified Path.

### EXAMPLE 2
```
Get-WinGetManifest -Path "C:\AppManifests\Microsoft.PowerToys\Microsoft.PowerToys.json"
```

Returns a Manifest object (*.json) of the specified JSON file.

### EXAMPLE 3
```
Get-WinGetManifest -FunctionName "PrivateSource" -ManifestIdentifier "Windows.PowerToys"
```

Returns a Manifest object of the specified Manifest Identifier that is queried against in the Rest APIs.

### EXAMPLE 4
```
Get-WinGetManifest -FunctionName "PrivateSource" -ManifestIdentifier "Windows.PowerToys" -SubscriptionName "Visual Studio Subscription"
```

Returns a Manifest object of the specified Manifest Identifier that is queried against in the Rest APIs from the specified Subscription Name.

### EXAMPLE 5
```
Get-WinGetManifest -FunctionName "PrivateSource"
```

Returns an array of Manifest objects that are found in the specified Azure Function.

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

### -URL
Web URL to the host site containing the Rest APIs with access key (if required).

```yaml
Type: String
Parameter Sets: Custom
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FunctionName
Name of the Azure Function Name that contains the Windows Package Manager Rest APIs.

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
\[Optional\] Name of the Azure Subscription that contains the Azure Function which contains the Rest APIs.

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
