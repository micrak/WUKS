# Windows Update Killer Service (WUKS)
WUKS is a Windows service which keeps Windows Update components always disabled.
The application consists of single executable (wuks.exe) that play two roles:
1. It works as installer when started in interactive mode (user account).
2. It works as service when run in non-interactive mode (local system account).

## Requirements
- .NET Framework 4.7.2 or newer

## Installation
Start wuks.exe when service is not installed yet.

## Uninstallation
Start wuks.exe when service is already installed.

## Status
Service status can be viewed in Windows "Services" application (services.msc).

## Events
Service events can be viewed in Windows "Event Viewer" application (eventvwr.msc).