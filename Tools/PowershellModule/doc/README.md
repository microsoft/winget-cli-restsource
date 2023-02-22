# Create a Windows Package Manager REST source

This section provides guidance on how to create a REST source for the Windows Package Manager. ISVs or Publishers may host and manage a rest source if they would like full control of the applications available in a source. An independently hosted source may choose to expose the read endpoints publicly or restrict access to specific IP address via the addition of a traffic shaping module.  The basic setups configured by the cmdlets and examples result in a source that is publicly readable but requires an authorization key to manage. 

Windows Package Manager offers a comprehensive package manager solution including a command line tool and a set of services for installing applications. For more general package submission information, see [submit packages to Windows Package Manager](https://docs.microsoft.com/windows/package-manager/package/).

There are two ways available for managing REST source repositories with Windows Package Manager:

- [Manage Windows Package Manager REST source with PowerShell](new-winget-rest-source-azure.md#manage-windows-package-manager-rest-source-with-powershell)
- [Manage Windows Package Manager REST source manually](new-winget-rest-source-azure.md#manage-windows-package-manager-rest-source-manually)