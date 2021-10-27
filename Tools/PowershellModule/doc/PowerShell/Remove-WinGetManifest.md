---
external help file: Microsoft.WinGet.Source-help.xml
Module Name: Microsoft.WinGet.Source
online version:
schema: 2.0.0
---

# Remove-WinGetManifest

## SYNOPSIS
Removes a Manifest file from the Azure rest source

## SYNTAX

### WinGet (Default)
```
Remove-WinGetManifest [[-ManifestIdentifier] <String>] [[-SubscriptionName] <String>] [<CommonParameters>]
```

### Custom
```
Remove-WinGetManifest [-URL] <String> [-Key] <String> [[-ManifestIdentifier] <String>]
 [[-SubscriptionName] <String>] [<CommonParameters>]
```

### Azure
```
Remove-WinGetManifest [-FunctionName] <String> [[-ManifestIdentifier] <String>] [[-SubscriptionName] <String>]
 [<CommonParameters>]
```

## DESCRIPTION
By running this function with the required inputs, it will connect to the Azure Tenant that hosts the Windows Package Manager rest source, then removes the application Manifest.
    
The following Azure Modules are used by this script:
    Az.Resources
    Az.Accounts
    Az.Websites
    Az.Functions

## EXAMPLES

### EXAMPLE 1
```
Remove-WinGetManifest -FunctionName "PrivateSource" -ManifestIdentifier "Windows.PowerToys"
```

Connects to Azure, then runs the Azure Function "PrivateSource" Rest APIs to remove the specified Manifest file from the Windows Package Manager rest source

### EXAMPLE 2
```
Remove-WinGetManifest -URL "https://contoso.azure.web.net/api/packageManifests" -ManifestIdentifier "Windows.PowerToys"
```

Connects to a remote URL Rest APIs to remove the Application Manifest from the Windows Package Manager rest source

## PARAMETERS

### -URL
Name of the URL that hosts the rest source.

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

### -Key
{{ Fill Key Description }}

```yaml
Type: String
Parameter Sets: Custom
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -FunctionName
Name of the Azure Function that hosts the rest source.

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
THe Manifest Id that represents the App Manifest to be removed.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 3
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] The Subscription name contains the Windows Package Manager rest source

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 4
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
