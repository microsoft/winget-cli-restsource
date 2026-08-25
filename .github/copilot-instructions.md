# WinGet REST Source Development Guide

## Project Overview

This repository is the reference implementation for a **Windows Package Manager REST source** — an Azure-hosted package source that a WinGet client can query. It includes both the deployable Azure Functions REST API and a PowerShell module for provisioning/managing REST source infrastructure.

- **`src/WinGet.RestSource.Functions`** - Azure Functions implementing the REST API endpoints (manifest search, get, information)
- **`src/WinGet.RestSource`** - Core models and business logic shared by the Functions app
- **`src/WinGet.RestSource.Infrastructure`** - Data access layer (CosmosDB)
- **`src/WinGet.RestSource.AppConfig`** - Application configuration
- **`src/WinGet.RestSource.PowershellSupport`** / **`Tools/PowershellModule`** - `Microsoft.WinGet.RestSource` PowerShell module for creating/managing REST sources on Azure
- **`src/WinGet.RestSource.UnitTest`** / **`IntegrationTest`** / **`Fuzzing`** - Test projects
- **`src/WinGet.RestSource.Utils`** - Shared utility code

## Building, Testing, and Running

### Prerequisites

- [Git LFS](https://git-lfs.github.com/)
- Visual Studio 2022 with **.NET desktop development**, **Azure development**, and **ASP.NET and web development** workloads

### Building

Open `src\WinGet.RestSource.sln` in Visual Studio and build. Command-line builds of the solution also work.

### Running Locally

1. Run `generate_self_sign_cert.ps1` in `src\WinGet.RestSource.Functions` to create a local HTTPS dev cert (used automatically via `launchSettings.json`).
2. Provision a CosmosDB instance (database `WinGet`, collection `Manifests` if using the ARM templates).
3. Copy `local.settings.template.json` to `local.settings.json` and populate connection info.
4. For fully anonymous local testing, set HttpTrigger level to `Anonymous` for `InformationGet`, `ManifestSearchPost`, and `ManifestGet`.
5. Run `WinGet.RestSource.Functions` via F5 in Visual Studio.
6. Add as a WinGet source: `winget source add -n "winget-pkgs-restsource" -a https://localhost:7071/api/ -t "Microsoft.Rest"`.

### Testing

- **Unit tests**: require the [Cosmos DB Emulator](https://docs.microsoft.com/azure/cosmos-db/local-emulator?tabs=ssl-netstd21); configure via `WinGet.RestSource.UnitTest\Test.runsettings.template.json` → `Test.runsettings.json`.
- **Integration tests**: require a deployed REST source endpoint and the [winget client](https://github.com/microsoft/winget-cli) installed locally; configure via `WinGet.RestSource.IntegrationTest\Test.runsettings.template.json`. Destructive/write tests are disabled by default (`RunWriteTests`).
- Run tests from Visual Studio Test Explorer (`Test > Run All Tests`).

## Architecture & Key Patterns

- **Azure Functions HTTP triggers** map directly to REST endpoints consumed by the WinGet client's `Microsoft.Rest` source type.
- **CosmosDB** is the backing store for manifest documents; access is abstracted through the Infrastructure layer.
- The **PowerShell module** (`Microsoft.WinGet.RestSource`) wraps ARM/Azure CLI operations to stand up the Functions app, CosmosDB, and supporting resources declaratively.
- REST endpoint contracts should stay in sync with the WinGet client's expectations in [microsoft/winget-cli](https://github.com/microsoft/winget-cli) — check client-side REST source parsing when changing response shapes.

## Naming Conventions

- C# code follows StyleCop rules in `stylecop.json`.
- Namespaces follow `Microsoft.WindowsPackageManager.Rest.<Area>`.

## Contributing

- Review `CONTRIBUTING.md` (root) for workflow and CLA requirements.
- Documentation for the PowerShell module lives in `Tools/PowershellModule/doc/WingetRestSource.md`.
