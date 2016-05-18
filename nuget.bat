@echo off

reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsPath > nul 2>&1
if ERRORLEVEL 1 goto MissingMSBuildRegistry

for /f "skip=2 tokens=2,*" %%A in ('reg.exe query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0" /v MSBuildToolsPath') do SET MSBUILDDIR=%%B

IF NOT EXIST "%MSBUILDDIR%" goto MissingMSBuildToolsPath
IF NOT EXIST "%MSBUILDDIR%msbuild.exe" goto MissingMSBuildExe

"%MSBUILDDIR%msbuild.exe" /version

"%MSBUILDDIR%msbuild.exe" Configgy.sln /p:Configuration=Release
.\nuget.exe pack Configgy\Configgy.csproj -Prop Configuration=Release

"%MSBUILDDIR%msbuild.exe" Configgy.Encrypter.sln /p:Configuration=Release
.\nuget.exe pack Configgy.Encrypter\Configgy.Encrypter.csproj -Prop Configuration=Release

:Exit
pause
exit

::ERRORS
::---------------------
:MissingMSBuildRegistry
echo Cannot obtain path to MSBuild tools from registry
goto Exit

:MissingMSBuildToolsPath
echo The MSBuild tools path from the registry '%MSBUILDDIR%' does not exist
goto Exit

:MissingMSBuildExe
echo The MSBuild executable could not be found in '%MSBUILDDIR%'
goto Exit