@echo off
set scripts=%~dp0
set scripts=%scripts:~0,-1%

set project=Configgy

if not defined configuration set configuration=Release

dotnet pack --include-symbols --configuration %configuration% --output %scripts%\.. %project%
if %errorlevel% neq 0 exit /b %errorlevel%
