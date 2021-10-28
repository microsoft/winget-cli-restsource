# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-ARMResourceName
{
    <#
    .SYNOPSIS
    Validates that the name that will be assigned to the Azure Resource will meet the resource types requirement.
    
    .DESCRIPTION
    Validates that the name that will be assigned to the Azure Resource will meet the resource types requirement.
        
    The following Azure Modules are used by this script:
        Az.Resources
        Az.Accounts
        Az.Websites
        Az.Functions

    .PARAMETER ResourceType
    The type of Azure Resource to validate requirements against.

    .PARAMETER ResourceName
    The name that the resource type will be validated against.

    .PARAMETER ARMObject
    The ARMObject provided by New-ARMParameterObjects. 

    .EXAMPLE
    Test-ARMResourceName -ARMObject $ARMObject

    Parses through the $ARMObject array, recalling this function for each object in the array validating that the name meets the Azure resource requirements.

    .EXAMPLE
    Test-ARMResourceName -ResourceType "AppInsight" -ResourceName "contoso0002"

    Verifies that the name "contoso0002" meets the requirements for Azure App Insights.
    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="Targetted")]
        [ValidateSet("AppInsight", "KeyVault", "StorageAccount", "asp", "CosmosDBAccount", "CosmosDBDatabase", "CosmosDBContainer", "Function", "FrontDoor")][String] $ResourceType,
        [Parameter(Position=1, Mandatory=$true, ParameterSetName="Targetted")][String] $ResourceName,
        [Parameter(Position=0, Mandatory=$true, ParameterSetName="SingleObject")] $ARMObject
    )
    BEGIN
    {
        ## Allows for a single instance of the ARM Object to be passed in
        if($PSCmdlet.ParameterSetName -eq "SingleObject") {
            ## Sets the required variables based on the ARM Object
            $ResourceType = $ARMObject.ObjectType
            $ResourceName = $ARMObject.ObjectName
        }

        ## Preset output experience
        $TextPaddingRight     = 30
        $TextPaddingRightChar = " "

        ## Creates an array of values to be compared against
        $LowerAlphabet  = @("a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z")
        $UpperAlphabet  = @("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z")
        $Numbers        = @("1", "2", "3", "4", "5", "6", "7", "8", "9", "0")
        $SpecialChar    = @("!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "+", "=", "{", "}", "[", "]", ":", ";", """", "'", "<", ",", ">", ".", "?", "/", "|", "\")

        ## Pre-Sets Values to False until proven otherwise
        $NameContainsHyphen                 = $false
        $NameContainsUnderscore             = $false
        $NameContainsConsecutiveHyphen      = $false
        $NameContainsConsecutiveunderscore  = $false
        $NameStartsWithLetter               = $false
        $NameStartsWithNumber               = $false
        $NameEndsWithLetter                 = $false
        $NameEndsWithNumber                 = $false
        $NameContainsLowerCase              = $false
        $NameContainsUpperCase              = $false
        $NameContainsNumber                 = $false
        $NameContainsSpecialChar            = $false
        $NameLengthInRange                  = $false
        $NameContainsSpaces                 = $false

        ## Sets the Values accordingly
        $Result = $False
        $NameLength = $ResourceName.Length
        $NameContainsHyphen     = $ResourceName.Contains("-")
        $NameContainsUnderscore = $ResourceName.Contains("_")
        $NameContainsSpaces     = $ResourceName.Contains(" ")
        $NameContainsConsecutiveHyphen     = $ResourceName.Contains("--")
        $NameContainsConsecutiveunderscore = $ResourceName.Contains("__")

        ## Validates if the name starts with, ends with or contains a number
        foreach($Number in $Numbers){if($ResourceName.StartsWith($Number)){$NameStartsWithNumber = $True}}
        foreach($Number in $Numbers){if($ResourceName.EndsWith($Number))  {$NameEndsWithNumber   = $True}}
        foreach($Number in $Numbers){if($ResourceName.Contains($Number))  {$NameContainsNumber   = $True}}

        ## Validates if the name starts with or ends with a letter
        foreach($Letter in $LowerAlphabet){if($ResourceName.ToLower().StartsWith($Letter)){$NameStartsWithLetter = $True}}
        foreach($Letter in $LowerAlphabet){if($ResourceName.ToLower().EndsWith($Letter))  {$NameEndsWithLetter   = $True}}

        ## Validates that the name contains an upper, lower or special characters
        foreach($Letter in $LowerAlphabet){if($ResourceName.Contains($Letter)){$NameContainsLowerCase   = $True}}
        foreach($Letter in $LowerAlphabet){if($ResourceName.Contains($Letter)){$NameContainsLowerCase   = $True}}
        foreach($Letter in $UpperAlphabet){if($ResourceName.Contains($Letter)){$NameContainsUpperCase   = $True}}
        foreach($Char   in $SpecialChar)  {if($ResourceName.Contains($Char))  {$NameContainsSpecialChar = $True}}

    }
    PROCESS
    {
        switch ($ResourceType) {
            "KeyVault" { 
                ## Alphanumerics, and Hyphens, starts with letter, ends with letter or number. Con't contain consecutive hyphens. Length: 3-24
                if($($NameLength -ge 3) -and $($NameLength -le 24)) {
                    $NameLengthInRange = $true
                }

                if($NameLengthInRange -and !$NameContainsSpecialChar -and !$NameStartsWithNumber -and !$NameContainsConsecutiveHyphen) {
                    $Result = $true
                }

                ## Outputs the tests to the screen and their status
                Write-Information -MessageData $("    Testing the ""$ResourceName"" name meets the requirements:")
                if($NameLengthInRange)              { Write-Verbose -Message $("  Name within Length:    ".PadRight($TextPaddingRight, $TextPaddingRightChar) +  $NameLengthInRange) }            else { Write-Warning -Message $("  Name within Length:    ".PadRight($TextPaddingRight, $TextPaddingRightChar) +  $NameLengthInRange) }
                if(!$NameContainsSpecialChar)       { Write-Verbose -Message $("  No Special Chars:      ".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsSpecialChar) }      else { Write-Warning -Message $("  No Special Chars:      ".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsSpecialChar) }
                if(!$NameStartsWithNumber)          { Write-Verbose -Message $("  Doesn't start with Num:".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameStartsWithNumber) }         else { Write-Warning -Message $("  Doesn't start with Num:".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameStartsWithNumber) }
                if(!$NameContainsConsecutiveHyphen) { Write-Verbose -Message $("  No consecutive hyphens:".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsConsecutiveHyphen)} else { Write-Warning -Message $("  No consecutive hyphens:".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsConsecutiveHyphen)}
                Write-Information -MessageData $("    -".PadRight($TextPaddingRight+6, "-"))
                Write-Information -MessageData $("      Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar)      +  $Result)
                Write-Information -MessageData $("")
            }
            "StorageAccount" { 
                ## Alphanumerics, Hyphens, and underscores, Length: 3-60
                if($($NameLength -ge 3) -and $($NameLength -le 60)) {
                    $NameLengthInRange = $true
                }

                if($NameLengthInRange -and !$NameContainsSpecialChar -and !$NameContainsUpperCase) {
                    $Result = $true
                }
                
                ## Outputs the tests to the screen and their status
                Write-Information -MessageData $("    Testing the ""$ResourceName"" name meets the requirements:")
                if($NameLengthInRange)        { Write-Verbose -Message $("  Name within length: ".PadRight($TextPaddingRight, $TextPaddingRightChar)   +  $NameLengthInRange) }       else { Write-Warning -Message $("  Name within length: ".PadRight($TextPaddingRight, $TextPaddingRightChar)   +  $NameLengthInRange) }
                if(!$NameContainsSpecialChar) { Write-Verbose -Message $("  No special chars:   ".PadRight($TextPaddingRight, $TextPaddingRightChar)   + !$NameContainsSpecialChar) } else { Write-Warning -Message $("  No special chars:   ".PadRight($TextPaddingRight, $TextPaddingRightChar)   + !$NameContainsSpecialChar) }
                if(!$NameContainsUpperCase)   { Write-Verbose -Message $("  No upper case chars:".PadRight($TextPaddingRight, $TextPaddingRightChar)   + !$NameContainsUpperCase) }   else { Write-Warning -Message $("  No upper case chars:".PadRight($TextPaddingRight, $TextPaddingRightChar)   + !$NameContainsUpperCase) }
                if(!$NameContainsSpaces)      { Write-Verbose -Message $("  No spaces:          ".PadRight($TextPaddingRight, $TextPaddingRightChar)   + !$NameContainsSpaces) }      else { Write-Warning -Message $("  No spaces:          ".PadRight($TextPaddingRight, $TextPaddingRightChar)   + !$NameContainsSpaces) }
                Write-Information -MessageData $("    -".PadRight($TextPaddingRight+6, "-"))
                Write-Information -MessageData $("      Validation Result:  ".PadRight($TextPaddingRight, $TextPaddingRightChar)   + $Result)
                Write-Information -MessageData $("")
            }
            "asp" {
                $NameLengthInRange = $true

                if($NameLengthInRange -and !$NameContainsSpaces) {
                    $Result = $true
                }

                ## Outputs the tests to the screen and their status
                Write-Information -MessageData $("    Testing the ""$ResourceName"" name meets the requirements:")
                if(!$NameContainsSpaces) { Write-Verbose -Message $("  No spaces:        ".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsSpaces) } else { Write-Warning -Message $("  No spaces:        ".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsSpaces) }
                Write-Information -MessageData $("    -".PadRight($TextPaddingRight+6, "-"))
                Write-Information -MessageData $("      Validation Result:".PadRight($TextPaddingRight, $TextPaddingRightChar) + $Result)
                Write-Information -MessageData $("")
            }
            "Function" { 
                ## Alphanumerics, Hyphens, and underscores, Length: 3-60
                if($($NameLength -ge 3) -and $($NameLength -le 63)) {
                    $NameLengthInRange = $true
                }

                if($NameLengthInRange -and !$NameContainsSpecialChar) {
                    $Result = $true
                }

                ## Outputs the tests to the screen and their status
                Write-Information -MessageData $("    Testing the ""$ResourceName"" name meets the requirements:")
                Write-Verbose -Message $("  Name within Length:".PadRight($TextPaddingRight, $TextPaddingRightChar) + $NameLengthInRange)
                Write-Verbose -Message $("  No Special Chars:  ".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsSpecialChar)
                Write-Information -MessageData $("-".PadRight($TextPaddingRight+6, "-"))
                Write-Information -MessageData $("      Validation Result: ".PadRight($TextPaddingRight, $TextPaddingRightChar) + $Result)
                Write-Information -MessageData $("")
            }
            "FrontDoor" {
                ## Alphanumerics and hyphens. Start and end with alphanumeric. Length: 5-64
                if($($NameLength -ge 5) -and $($NameLength -le 64)) {
                    $NameLengthInRange = $true
                }

                if($NameLengthInRange -and !$NameContainsSpecialChar) {
                    $Result = $true
                }

                ## Outputs the tests to the screen and their status
                Write-Information -MessageData $("    Testing the ""$ResourceName"" name meets the requirements:")
                Write-Verbose -Message $("  Name within Length:".PadRight($TextPaddingRight, $TextPaddingRightChar) + $NameLengthInRange)
                Write-Verbose -Message $("  No Special Chars:  ".PadRight($TextPaddingRight, $TextPaddingRightChar) + !$NameContainsSpecialChar)
                Write-Verbose -Message $("-".PadRight($TextPaddingRight+6, "-"))
                Write-Information -MessageData $("      Validation Result: ".PadRight($TextPaddingRight, $TextPaddingRightChar) + $Result)
                Write-Information -MessageData $("")
            }
            Default {
                $Result = $true

                Write-Information -MessageData $("    Testing the ""$ResourceName"" name meets the requirements:")
                Write-Information -MessageData $("      Validation Result: ".PadRight($TextPaddingRight, $TextPaddingRightChar) + $Result)
                Write-Information -MessageData $("")
            }
        }
    }
    END
    {
        Return $Result
    }
}