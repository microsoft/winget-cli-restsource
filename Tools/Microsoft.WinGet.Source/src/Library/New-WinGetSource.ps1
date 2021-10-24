Function New-WinGetSource
{
    <#
    .SYNOPSIS
    Creates a Windows Package Manager private repository in Azure for private storage of Windows Package Manager application Manifests.

    .DESCRIPTION
    Creates a Windows Package Manager private repository in Azure for private storage of Windows Package Manager application Manifests.

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
    [Optional] The Name of the Resource Group that the Windows Package Manager private source ARM Resources will be created in. (Default: WinGetPrivateSource)

    .PARAMETER Region
    [Optional] The Azure location where objects will be created in. (Default: westus)

    .PARAMETER WorkingDirectory
    [Optional] The directory where Parameter objects will be created in. (Default: Current Directory)

    .PARAMETER ARMFunctionPath
    [Optional] Path to the compiled Rest API Zip file. (Default: .\RestAPI\CompiledFunctions.ps1)

    .PARAMETER ImplementationPerformance
    [Optional] ["Demo", "Basic", "Enhanced"] specifies the performance of the resources to be created for the Windows Package Manager private repository. (Default: Basic)

    .PARAMETER ShowConnectionInstructions
    [Optional] If specified, the instructions for connecting to the Windows Package Manager private source. (Default: False)

    .EXAMPLE
    New-WinGetSource -Name "contoso0002"

    Creates the Windows Package Manager private source in Azure with resources named "contoso0002" in the westus region of Azure with the basic level performance.

    .EXAMPLE
    New-WinGetSource -Name "contoso0002" -ResourceGroup "WinGetSource" -SubscriptionName "Visual Studio Subscription" -Region "westus" -WorkingDirectory "C:\WinGet" -ImplementationPerformance "Basic" -ShowConnectionInformation

    Creates the Windows Package Manager private source in Azure with resources named "contoso0002" in the westus region of Azure with the basic level performance in the "Visual Studio Subscription" Subscription. Displays the required command to connect the WinGet client to the new private repository after the repository has been created.

    #>
    PARAM(
        [Parameter(Position=0, Mandatory=$true)]  [string]$Name,
        [Parameter(Position=1, Mandatory=$false)] [string]$Index,
        [Parameter(Position=2, Mandatory=$false)] [string]$ResourceGroup = "WinGetPrivateSource",
        [Parameter(Position=3, Mandatory=$false)] [string]$SubscriptionName,
        [Parameter(Position=4, Mandatory=$false)] [string]$Region = "westus",
        [Parameter(Position=5, Mandatory=$false)] [string]$WorkingDirectory = $(Get-Location).Path,
        [Parameter(Position=6, Mandatory=$false)] [string]$ARMFunctionPath = "$PSScriptRoot\RestAPI\CompiledFunctions.zip",
        [ValidateSet("Demo", "Basic", "Enhanced")]
        [Parameter(Position=7, Mandatory=$false)] [string]$ImplementationPerformance = "Basic",
        [Parameter()] [switch]$ShowConnectionInstructions
    )
    BEGIN
    {
        if($ImplementationPerformance -eq "Demo") {
            Write-Warning -Message "`n The ""Demo"" build creates a free-tier Azure Cosmos DB Account. Only 1 Cosmos DB Account per tenant can make use of this.`n`n"
        }
        
        ## Paths to the Parameter and Template folders and the location of the Function Zip
        $ParameterFolderPath = "$WorkingDirectory\Parameters"
        $TemplateFolderPath  = "$WorkingDirectory\Templates"

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
#        $ResultTemplates = New-Item -ItemType Directory -Path $TemplateFolderPath -ErrorAction SilentlyContinue -InformationAction SilentlyContinue
        if($ResultParameter) { 
            Write-Verbose -Message "Created Directory to contain the ARM Parameter files ($($ResultParameter.FullName))." 
        }
#        if($ResultTemplates) { 
#            Write-Verbose -Message "Created Directory to contain the ARM Template files ($($ResultTemplates.FullName))." 
#        }
        
        ###############################
        ## Creates the ARM files
        $ARMObjects = New-ARMParameterObject -ParameterFolderPath $ParameterFolderPath -TemplateFolderPath $TemplateFolderPath -Index $Index -Name $Name -Region $Region -ImplementationPerformance $ImplementationPerformance
        #New-ARMTemplateObject -Path $TemplateFolderPath

        ###############################
        ## Connects to Azure, if not already connected.
        Write-Verbose -Message "Testing connection to Azure."
        
        $Result = Connect-ToAzure -SubscriptionName $SubscriptionName
        if(!($Result)) {
            throw "Failed to connect to Azure. Please run Connect-AzAccount to connect to Azure, or re-run the cmdlet and enter your credentials."
        }


        ###############################
        ## Create Resource Group 
        Write-Verbose -Message "Creating the Resource Group used to host the Windows Package Manager private source."
        Add-AzureResourceGroup -Name $ResourceGroup -Region $Region
        
        #### Verifies ARM Parameters are correct ####
        $Result = Test-ARMTemplate -ARMObjects $ARMObjects -ResourceGroup $ResourceGroup -ErrorAction SilentlyContinue -ErrorVariable err

        if($err){
            $ErrReturnObject = @{
                ARMObjects    = $ARMObjects
                ResourceGroup = $ResourceGroup
                Result        = $Result
            }
            
            Write-Error -Message "Testing found an error with the ARM Objects." -TargetObject $ErrReturnObject
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
        New-ARMObjects -ARMObjects $ARMObjects -ArchiveFunctionZip $ARMFunctionPath -AzResourceGroup $ResourceGroup

        ###############################
        ## Shows how to connect local Windows Package Manager Client to newly created private repository
        if($ShowConnectionInstructions) {
            #$AzSubscriptionName = $(Get-AzContext).Subscription.Name
            $jsonFunction       = Get-Content -Path $($ARMObjects.Where({$_.ObjectType -eq "Function"}).ParameterPath) | ConvertFrom-Json
            $AzFunctionName     = $jsonFunction.Parameters.FunctionName.Value
            $AzFunctionURL      = $(Get-AzFunctionApp -Name $AzFunctionName -ResourceGroupName $ResourceGroup).DefaultHostName

            ## Post script Run Informational:
            #### Instructions on how to add the repository to your Windows Package Manager Client
            Write-Information -MessageData "Use the following command to register the new private repository with your Windows Package Manager Client:"
            Write-Information -MessageData "  winget source add -n ""PrivateRepo"" -a ""https://$AzFunctionURL/api/"" -t ""Microsoft.Rest"""

            #### For more information about how to use the solution, visit the aka.ms link.
            Write-Information -MessageData "`n  For more information on the Windows Package Manager Client, go to: https://aka.ms/winget-command-help`n"
        }
    }
    END
    {
        Return $ARMObjects
    }
}