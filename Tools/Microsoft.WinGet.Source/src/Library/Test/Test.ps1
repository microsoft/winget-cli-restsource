Add-Type -Path "C:\Users\jamespik\Desktop\Test\Microsoft.Winget.PowershellSupport.dll";

$json = [Microsoft.WinGet.RestSource.PowershellSupport.YamlToRestConverter]::AddManifestToPackageManifest("C:\Users\jamespik\source\repos\microsoft\winget-pkgs\manifests\c\CarlWenrich\PythonTkGuiBuilder\1.0.0", "");

return $json;

