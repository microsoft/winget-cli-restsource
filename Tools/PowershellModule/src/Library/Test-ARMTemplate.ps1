# Copyright (c) Microsoft Corporation.
# Licensed under the MIT License.
Function Test-ARMTemplate
{
    <#
    .SYNOPSIS
    Validates that the parameter files have been build correctly, matches to the template files, and can be used to build Azure resources. Will also validate that the naming used for the resources is available, and meets requirements. Returns boolean.

    .DESCRIPTION
    Validates that the parameter files have been build correctly, matches to the template files, and can be used to build Azure resources. Will also validate that the naming used for the resources is available, and meets requirements. Returns boolean.
        - True, if the validation testing passes
        - False, if the validation testing fails.

    .PARAMETER ARMObjects
    Object Returned from the Get-ARMParameterObject.

    .PARAMETER ResourceGroup
    The Resource Group that the objects will be tested in reference to.

    .EXAMPLE
    Test-ARMTemplate -ARMObjects $ARMObjects -ResourceGroup "WinGetResourceGroup"

    Tests that the Azure Resource can be created in the specified Azure Resource Group with the parameter and template files.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)] $ARMObjects,
        [Parameter(Position=1, Mandatory=$true)] $ResourceGroup
    )
    BEGIN
    {
        Write-Information -MessageData "Verifying the ARM Resource Templates and Parameters are valid:"
        $Return = @()
    }
    PROCESS
    {
        ## Parses through all ARM Parameter objects to validate they are properly configured.
        foreach($Object in $ARMObjects)
        {
            ## Validates that each ARM object will work.
            Write-Information -MessageData "  Validation testing on ARM Resource ($($Object.ObjectType))."
            $AzResourceResult = Test-AzResourceGroupDeployment -ResourceGroupName $ResourceGroup -Mode Complete -TemplateFile $Object.TemplatePath -TemplateParameterFile $Object.ParameterPath
            $AzNameResult     = Test-ARMResourceName -ARMObject $Object

            ## If the ARM object fails validation, report error to screen.
            if($AzResourceResult -ne "" -or !$AzNameResult) { 
                $ErrReturnObject = @{
                    Test     = $AzResourceResult
                    ARMObject = $Object
                }

                if($AzResourceResult -ne "") {
                    if($AzNameResult) { 
                        Write-Verbose "Name is already in use."
                        Write-Error -Message "$($Object.ObjectType) Name is already in use, or there is an error with the Parameter file" -TargetObject $ErrReturnObject
                    }
                    else {
                        Write-Error -Message "$($Object.ObjectType) Name does not meet the requirements" -TargetObject $ErrReturnObject
                    }
                }
                ElseIF(!$AzNameResult)
                {
                    Write-Error "$($Object.ObjectType) Name does not meet the requirements." -TargetObject $ErrReturnObject
                }

                $TestResult = @{
                    ObjectType = $Object.ObjectType
                    ObjectName = $Object.Parameters.Parameters.Name
                    Result     = $Result
                }
                $Return += $TestResult
            }
        }
    }
    End
    {
        ## Returns the TestResults.
        Return $Return
    }
}