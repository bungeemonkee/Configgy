@echo off
set scripts=%~dp0
set scripts=%scripts:~0,-1%

set solution=Configgy.sln

if not defined configuration set configuration=Release

dotnet build -c %configuration% %solution%
