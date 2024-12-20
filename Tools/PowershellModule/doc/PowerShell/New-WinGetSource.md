---
external help file: Microsoft.WinGet.RestSource-help.xml
Module Name: microsoft.WinGet.RestSource
online version:
schema: 2.0.0
---

# New-WinGetSource

## SYNOPSIS
Creates a Windows Package Manager REST source in Azure.

## SYNTAX

```
New-WinGetSource [-Name] <String> [-ResourceGroup <String>] [-SubscriptionName <String>] [-Region <String>]
 [-TemplateFolderPath <String>] [-ParameterOutputPath <String>] [-RestSourcePath <String>]
 [-PublisherName <String>] [-PublisherEmail <String>] [-ImplementationPerformance <String>]
 [-RestSourceAuthentication <String>] [-CreateNewMicrosoftEntraIdAppRegistration]
 [-MicrosoftEntraIdResource <String>] [-MicrosoftEntraIdResourceScope <String>] [-ShowConnectionInstructions]
 [-MaxRetryCount <Int32>] [-ProgressAction <ActionPreference>] [<CommonParameters>]
```

## DESCRIPTION
Creates a Windows Package Manager REST source in Azure.

## EXAMPLES

### EXAMPLE 1
```
New-WinGetSource -Name "contosorestsource" -InformationAction Continue -Verbose
```

Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of 
Azure with the basic level performance.

### EXAMPLE 2
```
New-WinGetSource -Name "contosorestsource" -ResourceGroup "WinGet" -SubscriptionName "Visual Studio Subscription" -Region "westus" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic" -ShowConnectionInformation -InformationAction Continue -Verbose
```

Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of 
Azure with the basic level performance in the "Visual Studio Subscription" subscription.
Displays the required command to connect the WinGet client to the new Windows Package Manager REST source after the source has been created.

### EXAMPLE 3
```
New-WinGetSource -Name "contosorestsource" -RestSourceAuthentication "MicrosoftEntraId" -CreateNewMicrosoftEntraIdAppRegistration -ShowConnectionInformation -InformationAction Continue -Verbose
```

Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of 
Azure with the basic level performance.
The Windows Package Manager REST source is protected with Microsoft Entra Id authentication. 
A new Microsoft Entra Id app registration is created.

### EXAMPLE 4
```
New-WinGetSource -Name "contosorestsource" -RestSourceAuthentication "MicrosoftEntraId" -MicrosoftEntraIdResource "GUID" -MicrosoftEntraIdResourceScope "user-impersonation" -ShowConnectionInformation -InformationAction Continue -Verbose
```

Creates the Windows Package Manager REST source in Azure with resources named "contosorestsource" in the westus region of 
Azure with the basic level performance.
The Windows Package Manager REST source is protected with Microsoft Entra Id authentication. 
Uses existing Microsoft Entra Id app registration.

## PARAMETERS

### -Name
The base name of Azure Resources (Windows Package Manager REST source components) that will be created.

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

### -ResourceGroup
\[Optional\] The name of the Resource Group that the Windows Package Manager REST source will reside.
All Azure
Resources will be created in in this Resource Group (Default: WinGetRestSource)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
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
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Region
\[Optional\] The Azure location where Azure Resources (Windows Package Manager REST source components) will be created in.
(Default: westus)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: Westus
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
Position: Named
Default value: "$PSScriptRoot\..\Data\ARMTemplates"
Accept pipeline input: False
Accept wildcard characters: False
```

### -ParameterOutputPath
\[Optional\] The directory where ARM Template Parameter files will be created in.
(Default: Current Directory\Parameters)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
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
Position: Named
Default value: "$PSScriptRoot\..\Data\WinGet.RestSource.Functions.zip"
Accept pipeline input: False
Accept wildcard characters: False
```

### -PublisherName
\[Optional\] The Windows Package Manager REST source publisher name.
(Default: Signed in user email Or WinGetRestSource@DomainName)

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

### -PublisherEmail
\[Optional\] The Windows Package Manager REST source publisher email.
(Default: Signed in user email Or WinGetRestSource@DomainName)

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

### -ImplementationPerformance
\[Optional\] \["Developer", "Basic", "Enhanced"\] specifies the performance of the Azure Resources to be created for the Windows Package Manager REST source.
| Preference | Description                                                                                                             |
|------------|-------------------------------------------------------------------------------------------------------------------------|
| Developer  | Specifies lowest cost for developing the Windows Package Manager REST source. Uses free-tier options when available.    |
| Basic      | Specifies a basic functioning Windows Package Manager REST source.                                                      |
| Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers.                               |

(Default: Basic)

```yaml
Type: String
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
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
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -CreateNewMicrosoftEntraIdAppRegistration
\[Optional\] If specified, a new Microsoft Entra Id app registration will be created.
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

### -MicrosoftEntraIdResource
\[Optional\] Microsoft Entra Id authentication resource.
(Default: None)

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

### -MicrosoftEntraIdResourceScope
\[Optional\] Microsoft Entra Id authentication resource scope.
(Default: None)

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

### -ShowConnectionInstructions
\[Optional\] If specified, shows the instructions for connecting to the Windows Package Manager REST source.
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

### -MaxRetryCount
\[Optional\] Max ARM Templates deployment retry count upon failure.
(Default: 5)

```yaml
Type: Int32
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: 5
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

## NOTES

## RELATED LINKS
