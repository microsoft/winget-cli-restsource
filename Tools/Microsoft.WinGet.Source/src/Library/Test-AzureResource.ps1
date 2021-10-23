
Function Test-AzureResource
{
    PARAM(
        [Parameter(Position=0, Mandatory = $false)] [string]$ResourceGroup,
        [Parameter(Position=1, Mandatory = $false)] [string]$FunctionName,
        [Parameter(Position=2, Mandatory = $false)] [switch]$VerboseLogging
    )
    BEGIN
    {
        $Result = $true
        $AzureResourceGroupName = $ResourceGroup
        $AzureFunctionName = $FunctionName
        
        $AzureFunctionNameNotNullOrEmpty = $true
        $AzureFunctionNameExists         = $true

        $AzureResourceGroupNameNotNullOrEmpty = $true
        $AzureResourceGroupNameExists         = $true

        ## Determines if the Azure Function App Name is not null or empty
        if($AzureFunctionName.Length -le 0) {
            $AzureFunctionName               = "<null>"
            $AzureFunctionNameNotNullOrEmpty = $false 
        }
    
        ## Determines if the Azure Function App Name is in Azure
        if($(Get-AzFunctionApp).Where({$_.Name -eq $AzureFunctionName}).Count -le 0) {
            $AzureFunctionNameExists = $false
        }
        
        ##Determines if the Azure Resource Group Name is not null or empty
        if($AzureResourceGroupName.Length -le 0) {
            $AzureResourceGroupName               = "<null>"
            $AzureResourceGroupNameNotNullOrEmpty = $false 
        }

        if($(Get-AzResourceGroup).Where({$_.ResourceGroupName -eq $AzureResourceGroupName}).Count -lt 0) {
            $AzureResourceGroupNameExists = $false
        }
   }
    PROCESS
    {
        Write-Verbose -Message "Azure Resources:`n           Azure Function Exists:       $AzureFunctionNameExists`n           Azure Resource Group Exists: $AzureResourceGroupNameExists"

        ## If either the Azure Function Name or the Azure Resource Group Name are null, error.
        if(!$AzureFunctionNameNotNullOrEmpty -or !$AzureResourceGroupNameNotNullOrEmpty -or !$AzureFunctionNameExists -or !$AzureResourceGroupNameExists) {
            $ErrReturnObject = @{
                AzureFunctionNameNotNullOrEmpty      = $AzureFunctionNameNotNullOrEmpty
                AzureResourceGroupNameNotNullOrEmpty = $AzureResourceGroupNameNotNullOrEmpty
                AzureFunctionNameExists              = $AzureFunctionNameExists
                AzureResourceGroupNameExists         = $AzureResourceGroupNameExists
                Result                               = $false
            }

            Write-Error -Message "Both the Azure Function and Resource Group Names can not be null and must exist. Please verify that the Azure function and Resource Group exist." -Category InvalidArgument -TargetObject $ErrReturnObject
            $Result = $false
        }
    }
    END
    {
        return $Result
    }
}