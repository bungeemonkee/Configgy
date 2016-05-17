msbuild Configgy.sln /p:Configuration=Release
.\nuget.exe pack Configgy\Configgy.csproj -Prop Configuration=Release