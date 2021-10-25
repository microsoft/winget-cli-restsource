---
external help file: Microsoft.WinGet.Source-help.xml
Module Name: Microsoft.WinGet.Source
online version:
schema: 2.0.0
---

# Add-WinGetManifest

## SYNOPSIS
Submits a Manifest file(s) to the Azure private source

## SYNTAX

```
Add-WinGetManifest [-FunctionName] <String> [-Path] <String> [[-SubscriptionName] <String>]
 [<CommonParameters>]
```

## DESCRIPTION
By running this function with the required inputs, it will connect to the Azure Tennant that hosts the Windows Package Manager private source, then collects the required URL for Manifest submission before retrieving the contents of the Manifest JSON to submit.
    
The following Azure Modules are used by this script:
    Az.Resources
    Az.Accounts
    Az.Websites
    Az.Functions

## EXAMPLES

### EXAMPLE 1
```
Add-WinGetManifest -FunctionName "PrivateSource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json"
```

Connects to Azure, then runs the Azure Function "PrivateSource" Rest APIs to add the specified Manifest file (*.json) to the Windows Package Manager private source

### EXAMPLE 2
```
Add-WinGetManifest -FunctionName "PrivateSource" -Path "C:\AppManifests\Microsoft.PowerToys\"
```

Connects to Azure, then runs the Azure Function "PrivateSource" Rest APIs to adds the Manifest file(s) (*.json / *.yaml) found in the specified folder to the Windows Package Manager private source

### EXAMPLE 3
```
Add-WinGetManifest -FunctionName "PrivateSource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json" -SubscriptionName "Visual Studio Subscription"
```

Connects to Azure and the specified Subscription, then runs the Azure Function "PrivateSource" Rest APIs to add the specified Manifest file (*.json) to the Windows Package Manager private source

## PARAMETERS

### -FunctionName
Name of the Azure Function that hosts the private source.

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

### -Path
The Path to the JSON manifest file or folder hosting the JSON / YAML files that will be uploaded to the private source.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] The Subscription name contains the Windows Package Manager private source

```yaml
Type: String
Parameter Sets: (All)
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
