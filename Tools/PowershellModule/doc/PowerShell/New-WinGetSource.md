---
external help file: Microsoft.WinGet.Source-help.xml
Module Name: Microsoft.WinGet.Source
online version:
schema: 2.0.0
---

# New-WinGetSource

## SYNOPSIS
Creates a Windows Package Manager rest source in Azure for private storage of Windows Package Manager application Manifests.

## SYNTAX

```
New-WinGetSource [-Name] <String> [[-Index] <String>] [[-ResourceGroup] <String>]
 [[-SubscriptionName] <String>] [[-Region] <String>] [[-WorkingDirectory] <String>]
 [[-ARMFunctionPath] <String>] [[-ImplementationPerformance] <String>] [-ShowConnectionInstructions]
 [<CommonParameters>]
```

## DESCRIPTION
Creates a Windows Package Manager rest source in Azure for private storage of Windows Package Manager application Manifests.

The following Azure Modules are used by this script:
    Az.Resources
    Az.Accounts
    Az.Websites
    Az.Functions

## EXAMPLES

### EXAMPLE 1
```
New-WinGetSource -Name "contoso0002"
```

Creates the Windows Package Manager rest source in Azure with resources named "contoso0002" in the westus region of Azure with the basic level performance.

### EXAMPLE 2
```
New-WinGetSource -Name "contoso0002" -ResourceGroup "WinGetSource" -SubscriptionName "Visual Studio Subscription" -Region "westus" -WorkingDirectory "C:\WinGet" -ImplementationPerformance "Basic" -ShowConnectionInstructions
```

Creates the Windows Package Manager rest source in Azure with resources named "contoso0002" in the westus region of Azure with the basic level performance in the "Visual Studio Subscription" Subscription.
Displays the required command to connect the WinGet client to the new rest source after the repository has been created.

## PARAMETERS

### -Name
The name of the objects that will be created

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

### -Index
\[Optional\] The suffix that will be added to each name and file names.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ResourceGroup
\[Optional\] The Name of the Resource Group that the Windows Package Manager rest source will reside. All Azure resources will be created in in this Resource Group (Default: WinGetPrivateSource)
(Default: WinGetPrivateSource)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 3
Default value: WinGetPrivateSource
Accept pipeline input: False
Accept wildcard characters: False
```

### -SubscriptionName
{{ Fill SubscriptionName Description }}

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

### -Region
\[Optional\] The Azure location where objects will be created in.
(Default: westus)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 5
Default value: Westus
Accept pipeline input: False
Accept wildcard characters: False
```

### -WorkingDirectory
\[Optional\] The directory where Parameter objects will be created in.
(Default: Current Directory)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 6
Default value: $(Get-Location).Path
Accept pipeline input: False
Accept wildcard characters: False
```

### -ARMFunctionPath
\[Optional\] Path to the compiled Rest API Zip file.
(Default: .\RestAPI\CompiledFunctions.ps1)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 7
Default value: "$PSScriptRoot\RestAPI\CompiledFunctions.zip"
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImplementationPerformance
\[Optional\] \["Demo", "Basic", "Enhanced"\] specifies the performance of the resources to be created for the Windows Package Manager rest source.
| Preference | Description                                                                                                             |
|------------|-------------------------------------------------------------------------------------------------------------------------|
| Demo       | Specifies lowest cost for demonstrating the Windows Package Manager rest source. Uses free-tier options when available. |
| Basic      | Specifies a basic functioning Windows Package Manager rest source. Low cost.                                            |
| Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers. High cost.                    |

(Default: Basic)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 8
Default value: Basic
Accept pipeline input: False
Accept wildcard characters: False
```

### -ShowConnectionInstructions
\[Optional\] If specified, the instructions for connecting to the Windows Package Manager rest source.
(Default: False)

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
