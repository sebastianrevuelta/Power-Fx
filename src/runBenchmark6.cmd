@echo off
if not exist "BenchmarkDotNet.Artifacts" md "BenchmarkDotNet.Artifacts"
wmic cpu get Caption, CurrentClockSpeed, Name, NumberOfCores, NumberOfLogicalProcessors > BenchmarkDotNet.Artifacts\cpu.csv
wmic computersystem get TotalPhysicalMemory > BenchmarkDotNet.Artifacts\memory.csv
for /f %%a in ('dotnet --list-sdks ^| findstr 6\.0') do set dotnetver=%%a
set MSBuildSDKsPath=C:\Program Files\dotnet\sdk\%dotnetver%\sdks
if not exist global.json dotnet new globaljson --sdk-version %dotnetver%
"tests\Microsoft.PowerFx.Performance.Tests\bin\Release\net60\Microsoft.PowerFx.Performance.Net6.Tests.exe" --list tree
"tests\Microsoft.PowerFx.Performance.Tests\bin\Release\net60\Microsoft.PowerFx.Performance.Net6.Tests.exe" -f *
del global.json
