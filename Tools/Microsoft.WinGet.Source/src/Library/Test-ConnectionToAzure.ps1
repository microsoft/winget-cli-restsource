
Function Test-ConnectionToAzure
{
    PARAM(
        [Parameter(Position=0, Mandatory=$false)] [string] $SubscriptionName,
        [Parameter(Position=1, Mandatory=$false)] [string] $SubscriptionId
    )
    BEGIN
    {
        $Result    = $false
        $AzContext = Get-AzContext
    }
    PROCESS
    {
        if($AzContext) {
            Write-Verbose -Message "Connected to Azure"
            $Result = $true

            if($AzContext.Subscription.Name -ne $SubscriptionName -and $SubscriptionName) {
                ## If Subscription Name paramter is passed in, and the value doesn't match current connection return $false
                Write-Verbose -Message "Connection to an unmatched Subscription in Azure. Not connected to $SubscriptionName"
                $Result = $false
            }
            if($AzContext.Subscription.Id -ne $SubscriptionId -and $SubscriptionId) {
                ## If Subscription Id paramter is passed in, and the value doesn't match current connection return $false
                Write-Verbose -Message "Connection to an unmatched Subscription in Azure. Not connected to $SubscriptionId"
                $Result = $false
            }
        }
        else {
            ## Not currently connected to Azure
            Write-Verbose -Message "Not connected to Azure, please connect to your Azure Subscription"
        }
    }
    END
    {
        Return $Result
    }
}