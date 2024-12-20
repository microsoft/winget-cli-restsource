---
external help file: Microsoft.WinGet.RestSource-help.xml
Module Name: microsoft.WinGet.RestSource
online version:
schema: 2.0.0
---

# Add-WinGetManifest

## SYNOPSIS
Submits a Manifest to the Windows Package Manager REST source.

## SYNTAX

```
Add-WinGetManifest [-FunctionName] <String> [-Path] <String> [-SubscriptionName <String>]
 [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION
Submits a Manifest to the Windows Package Manager REST source.  
Running this function will first connect to the Azure Tenant that hosts the Windows Package Manager REST source. 
The function will then collect the required URL before retrieving the contents of the Manifest for submission.

## EXAMPLES

### EXAMPLE 1
```
Add-WinGetManifest -FunctionName "contosorestsource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json"
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to add the specified Manifest (*.json) 
to the Windows Package Manager REST source.

### EXAMPLE 2
```
Add-WinGetManifest -FunctionName "contosorestsource" -Path "C:\AppManifests\Microsoft.PowerToys\"
```

Connects to Azure, then runs the Azure Function "contosorestsource" REST APIs to adds the Manifest (*.json / *.yaml) 
found in the specified folder to the Windows Package Manager REST source.

### EXAMPLE 3
```
Add-WinGetManifest -FunctionName "contosorestsource" -Path "C:\AppManifests\Microsoft.PowerToys\PowerToys.json" -SubscriptionName "Visual Studio Subscription"
```

Connects to Azure and the specified Subscription, then runs the Azure Function "contosorestsource" REST APIs to add the 
specified Manifest (*.json) to the Windows Package Manager REST source.

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

### -Path
Supports input from pipeline.
The path to the Manifest file or folder hosting either a JSON or YAML file(s) that will be uploaded to the REST source. 
This path may contain a single Manifest file, or a folder containing Manifest files for a single Manifest. Does not support 
Does not support targeting a single folder containing multiple different Manifests.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: True
Position: 2
Default value: None
Accept pipeline input: True (ByValue)
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

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

## NOTES

## RELATED LINKS
