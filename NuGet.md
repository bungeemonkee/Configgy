# NuGet

Notes for creating and updating the nuget package

## Create the package

To create the NuGet package:

1. Update the Configgy.nuspec as necessary
    * Pay special attention to the 'releaseNotes' element
    * Leave all the variables (the things inside $dollar signs$) alone - they are populated from metadata in AssemblyInfo.cs
2. Update the version numbers in AssemblyInfo.cs
3. Create the package
    1. Open a command line
    2. CD to the solution directory
    3. Run: `nuget.bat`
4. Upload the package to https://www.nuget.org/packages/upload

## Notes

* Nuget will include the `AssemblyInformationalVersion` if it exists, but will ignore it if it includes a SemVer build version as part of the prerelease identifier
    * This is allowed: 1.0.0-alpha1
    * But this is not: 1.0.0-alpha.1
