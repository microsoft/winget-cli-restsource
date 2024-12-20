---
external help file: Microsoft.WinGet.RestSource-help.xml
Module Name: microsoft.WinGet.RestSource
online version:
schema: 2.0.0
---

# Update-WinGetSource

## SYNOPSIS
Updates a Windows Package Manager REST source in Azure.

## SYNTAX

```
Update-WinGetSource [[-Name] <String>] [[-ResourceGroup] <String>] [[-SubscriptionName] <String>]
 [[-TemplateFolderPath] <String>] [[-ParameterOutputPath] <String>] [[-RestSourcePath] <String>]
 [[-PublisherName] <String>] [[-PublisherEmail] <String>] [[-ImplementationPerformance] <String>]
 [[-RestSourceAuthentication] <String>] [[-MicrosoftEntraIdResource] <String>]
 [[-MicrosoftEntraIdResourceScope] <String>] [[-MaxRetryCount] <Int32>] [[-FunctionName] <String>]
 [-PublishAzureFunctionOnly] [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION
Updates a Windows Package Manager REST source in Azure.  
The update operation will not be able to detect existing Windows Package Manager REST source configurations. 
To use existing configurations, put existing ARM Template Parameter files in ParameterOutputPath.

## EXAMPLES

### EXAMPLE 1
```
Update-WinGetSource -Name "contosorestsource" -InformationAction Continue -Verbose
```

Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance.

### EXAMPLE 2
```
Update-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -SubscriptionName "Visual Studio Subscription" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic" -InformationAction Continue -Verbose
```

Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance in the "Visual Studio Subscription" Subscription.

### EXAMPLE 3
```
Update-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -RestSourceAuthentication "MicrosoftEntraId" -MicrosoftEntraIdResource "GUID" -MicrosoftEntraIdResourceScope "user-impersonation" -InformationAction Continue -Verbose
```

Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with the basic level performance. 
Uses existing Microsoft Entra Id app registration.

### EXAMPLE 4
```
Update-WinGetSource -Name "contosorestsource" -PublishAzureFunctionOnly -InformationAction Continue -Verbose
```

Updates the Windows Package Manager REST source in Azure with resources named "contosorestsource" with publishing Azure Function only.

## PARAMETERS

### -Name
\[Optional\] The base name of Azure Resources (Windows Package Manager REST source components) that will be created.
Required if not PublishAzureFunctionOnly.

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ResourceGroup
\[Optional\] The name of the Resource Group that the Windows Package Manager REST source will reside.
All Azure
Resources will be updated in in this Resource Group (Default: WinGetRestSource)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 2
Default value: WinGetRestSource
Accept pipeline input: False
Accept wildcard characters: False
```

### -SubscriptionName
\[Optional\] The name of the subscription that will be used to host the Windows Package Manager REST source.
(Default: Current connected subscription)

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

### -TemplateFolderPath
\[Optional\] The directory containing required ARM templates.
(Default: $PSScriptRoot\..\Data\ARMTemplates)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 4
Default value: "$PSScriptRoot\..\Data\ARMTemplates"
Accept pipeline input: False
Accept wildcard characters: False
```

### -ParameterOutputPath
\[Optional\] The directory containing ARM Template Parameter files.
If existing ARM Template Parameter files are found, they will be used for Windows Package Manager REST source update without modification.
If ARM Template Parameter files are not found, new ARM Template Parameter files with default values will be created.
(Default: Current Directory\Parameters)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 5
Default value: "$($(Get-Location).Path)\Parameters"
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestSourcePath
\[Optional\] Path to the compiled Azure Function (Windows Package Manager REST source) Zip file.
(Default: $PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 6
Default value: "$PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip"
Accept pipeline input: False
Accept wildcard characters: False
```

### -PublisherName
\[Optional\] The WinGet rest source publisher name.
(Default: Signed in user email Or WinGetRestSource@DomainName)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 7
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PublisherEmail
\[Optional\] The WinGet rest source publisher email.
(Default: Signed in user email Or WinGetRestSource@DomainName)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 8
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ImplementationPerformance
\[Optional\] \["Developer", "Basic", "Enhanced"\] specifies the performance of the Azure Resources to be created for the Windows Package Manager REST source.
| Preference | Description                                                                                                             |
|------------|-------------------------------------------------------------------------------------------------------------------------|
| Developer  | Specifies lowest cost for developing the Windows Package Manager REST source. Uses free-tier options when available.    |
| Basic      | Specifies a basic functioning Windows Package Manager REST source.                                                      |
| Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers.                               |

Note for updating Windows Package Manager REST source, performance downgrading is not allowed by Azure Resources used in Windows Package Manager REST source.

(Default: Basic)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 9
Default value: Basic
Accept pipeline input: False
Accept wildcard characters: False
```

### -RestSourceAuthentication
\[Optional\] \["None", "MicrosoftEntraId"\] The Windows Package Manager REST source authentication type.
(Default: None)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 10
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MicrosoftEntraIdResource
\[Optional\] Microsoft Entra Id authentication resource.
(Default: None)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 11
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MicrosoftEntraIdResourceScope
\[Optional\] Microsoft Entra Id authentication resource scope.
(Default: None)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 12
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -MaxRetryCount
\[Optional\] Max ARM Templates deployment retry count upon failure.
(Default: 3)

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: 13
Default value: 3
Accept pipeline input: False
Accept wildcard characters: False
```

### -FunctionName
\[Optional\] The Azure Function name.
Required if PublishAzureFunctionOnly is specified.
(Default: None)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: 14
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -PublishAzureFunctionOnly
\[Optional\] If specified, only performs Azure Function publish.
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
