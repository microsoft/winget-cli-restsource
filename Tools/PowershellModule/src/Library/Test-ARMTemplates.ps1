# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-ARMTemplates
{
    <#
    .SYNOPSIS
    Validates that the parameter files have been build correctly, matches to the template files, and can be used to build Azure 
    resources. Will also validate that the naming used for the resources is available, and meets requirements. Returns a list of
    failed validations. If all validations pass, returns empty.

    .DESCRIPTION
    Validates that the parameter files have been build correctly, matches to the template files, and can be used to build Azure 
    resources. Will also validate that the naming used for the resources is available, and meets requirements. Returns a list of
    failed validations. If all validations pass, returns empty.

    .PARAMETER ARMObjects
    Object Returned from the New-ARMParameterObjects.

    .PARAMETER ResourceGroup
    The Resource Group that the objects will be tested in reference to.

    .EXAMPLE
    Test-ARMTemplates -ARMObjects $ARMObjects -ResourceGroup "WinGet"

    Tests that the Azure Resource can be created in the specified Azure Resource Group with the parameter and template files.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] [array] $ARMObjects,
        [Parameter(Position=1, Mandatory=$true)] [string] $ResourceGroup
    )

    Write-Information "Verifying the ARM Resource Templates and Parameters are valid:"
    [PSCustomObject[]]$Return = @()

    ## Parses through all ARM Parameter objects to validate they are properly configured.
    foreach($Object in $ARMObjects)
    {
        ## Validates that each ARM object will work.
        Write-Information "Validation testing on ARM Resource ($($Object.ObjectType))."
        $AzResourceResult = Test-AzResourceGroupDeployment -ResourceGroupName $ResourceGroup -Mode Complete -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath

        ## If the ARM object fails validation, report error to screen.
        if($AzResourceResult) { 
            [PSCustomObject]$ErrReturnObject = [PSCustomObject]@{
                ARMObject = $Object
                TestResult = $AzResourceResult
            }

            Write-Error -Message "Failed to validate ARM Template with Template parameters. Template: $($Object.TemplatePath), Parameters: $($Object.ParameterPath)" -TargetObject $ErrReturnObject

            $Return += $ErrReturnObject
        }
    }

    ## Returns the TestResults.
    return $Return
}