Function New-WinGetSource
{
    <#
    .SYNOPSIS
    Creates a Windows Package Manager rest source in Azure for the storage of Windows Package Manager application Manifests.

    .DESCRIPTION
    Creates a Windows Package Manager rest source in Azure for the storage of Windows Package Manager application Manifests.

    The following Azure Modules are used by this script:
        Az.Resources
        Az.Accounts
        Az.Websites
        Az.Functions

    .PARAMETER Name
    The name of the objects that will be created

    .PARAMETER Index
    [Optional] The suffix that will be added to each name and file names.

    .PARAMETER ResourceGroup
    [Optional] The name of the Resource Group that the Windows Package Manager rest source will reside. All Azure resources will be created in in this Resource Group (Default: WinGetrestsource)

    .PARAMETER SubscriptionName
    [Optional] The name of the subscription that will be used to host the Windows Package Manager rest source.

    .PARAMETER Region
    [Optional] The Azure location where objects will be created in. (Default: westus)

    .PARAMETER ParameterOutput
    [Optional] The directory where Parameter objects will be created in. (Default: Current Directory)

    .PARAMETER RestSourcePath
    [Optional] Path to the compiled Rest API Zip file. (Default: .\RestAPI\CompiledFunctions.ps1)

    .PARAMETER ImplementationPerformance
    [Optional] ["Demo", "Basic", "Enhanced"] specifies the performance of the resources to be created for the Windows Package Manager rest source.
    | Preference | Description                                                                                                             |
    |------------|-------------------------------------------------------------------------------------------------------------------------|
    | Demo       | Specifies lowest cost for demonstrating the Windows Package Manager rest source. Uses free-tier options when available. |
    | Basic      | Specifies a basic functioning Windows Package Manager rest source. Low cost.                                            |
    | Enhanced   | Specifies a higher tier functionality with data replication across multiple data centers. High cost.                    |
    
    (Default: Basic)

    .PARAMETER ShowConnectionInstructions
    [Optional] If specified, the instructions for connecting to the Windows Package Manager rest source. (Default: False)

    .EXAMPLE
    New-WinGetSource -Name "contoso0002"

    Creates the Windows Package Manager rest source in Azure with resources named "contoso0002" in the westus region of Azure with the basic level performance.

    .EXAMPLE
    New-WinGetSource -Name "contoso0002" -ResourceGroup "WinGetSource" -SubscriptionName "Visual Studio Subscription" -Region "westus" -ParameterOutput "C:\WinGet" -ImplementationPerformance "Basic" -ShowConnectionInformation

    Creates the Windows Package Manager rest source in Azure with resources named "contoso0002" in the westus region of Azure with the basic level performance in the "Visual Studio Subscription" Subscription. Displays the required command to connect the WinGet client to the new rest source after the rest source has been created.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$Name,
        [Parameter(Position=1, Mandatory=$false)] [string]$Index,
        [Parameter(Position=2, Mandatory=$false)] [string]$ResourceGroup = "WinGetRestSource",
        [Parameter(Position=3, Mandatory=$false)] [string]$SubscriptionName,
        [Parameter(Position=4, Mandatory=$false)] [string]$Region = "westus",
        [Parameter(Position=5, Mandatory=$false)] [string]$ParameterOutput = $(Get-Location).Path,
        [Parameter(Position=6, Mandatory=$false)] [string]$RestSourcePath = "$PSScriptRoot\RestAPI\WinGet.RestSource.Functions.zip",
        [ValidateSet("Demo", "Basic", "Enhanced")]
        [Parameter(Position=7, Mandatory=$false)] [string]$ImplementationPerformance = "Basic",
        [Parameter()] [switch]$ShowConnectionInstructions
    )
    BEGIN
    {
        if($ImplementationPerformance -eq "Demo") {
            #Write-Warning -Message "`n The ""Demo"" build creates a free-tier Azure Cosmos DB Account. Only 1 Cosmos DB Account per tenant can make use of this.`n`n"
            Write-Warning -Message "`n The ""Demo"" build creates the Azure Cosmos DB Account with the ""Free-tier"" option selected which offset the total cost. Only 1 Cosmos DB Account per tenant can make use of this.`n`n"
        }
        
        ## Paths to the Parameter and Template folders and the location of the Function Zip
        $ParameterFolderPath = "$ParameterOutput\Parameters"
        $TemplateFolderPath  = "$PSScriptRoot\ARMTemplate"
        
        ## Outlines the Azure Modules that are required for this Function to work.
        $RequiredModules     = @("Az.Resources", "Az.Accounts", "Az.KeyVault","Az.Websites", "Az.Functions")
    }
    PROCESS
    {
        ###############################
        ## Validates that the Azure Modules are installed
        Write-Verbose -Message "Testing required PowerShell modules are installed."

        $RequiredModules = @("Az.Resources", "Az.Accounts", "Az.Websites", "Az.Functions")
        $Result = Test-PowerShellModuleExist -Modules $RequiredModules

        if(!$Result) {
            throw "Unable to run script, missing required PowerShell modules"
        }

        ###############################
        ## Create Folders for the Parameter and Template folder paths
        $ResultParameter = New-Item -ItemType Directory -Path $ParameterFolderPath -ErrorAction SilentlyContinue -InformationAction SilentlyContinue

        if($ResultParameter) { 
            Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($ResultParameter.FullName))." 
        }
        
        ###############################
        ## Creates the ARM files
        $ARMObjects = New-ARMParameterObject -ParameterFolderPath $ParameterFolderPath -TemplateFolderPath $TemplateFolderPath -Index $Index -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance

        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose -Message "Testing connection to Azure."
        
        $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
        if(!($Result)) {
            throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        }


        ###############################
        ## Create Resource Group 
        Write-Verbose -Message "Creating the Resource Group used to host the Windows Package Manager rest source."
        Add-AzureResourceGroup -Name $ResourceGroup -Region $Region
        
        #### Verifies ARM Parameters are correct ####
        $Result = Test-ARMTemplate -ARMObjects $ARMObjects -ResourceGroup $ResourceGroup -ErrorAction SilentlyContinue -ErrorVariable err

        if($err){
            $ErrReturnObject = @{
                ARMObjects    = $ARMObjects
                ResourceGroup = $ResourceGroup
                Result        = $Result
            }
            
            Write-Error -Message "Testing found an error with the ARM template or parameter files." -TargetObject $ErrReturnObject
        }


        ## If the attempt fails.. then exit.
        if($($Result)) {
            $ErrReturnObject = @{
                Result = $Result
            }

            Write-Error -Message "ARM Template and Parameter testing failed`n" -TargetObject $ErrReturnObject
            Return
        }

        ###############################
        ## Creates Azure Objects with ARM Templates and Parameters
        New-ARMObjects -ARMObjects $ARMObjects -RestSourcePath $RestSourcePath -AzResourceGroup $ResourceGroup

        ###############################
        ## Shows how to connect local Windows Package Manager Client to newly created rest source
        if($ShowConnectionInstructions) {
            $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
            $AzFunctionName     = $jsonFunction.Parameters.FunctionName.Value
            $AzFunctionURL      = $(Get-AzFunctionApp -Name $AzFunctionName -ResourceGroupName $ResourceGroup).DefaultHostName

            ## Post script Run Informational:
            #### Instructions on how to add the rest source to your Windows Package Manager Client
            Write-Information -MessageData "Use the following command to register the new rest source with your Windows Package Manager Client:"
            Write-Information -MessageData "  winget source add -n ""restsource"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest"""

            #### For more information about how to use the solution, visit the aka.ms link.
            Write-Information -MessageData "`n  For more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n"
        }
    }
    END
    {
        Return $ARMObjects
    }
}