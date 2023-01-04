// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using BenchmarkDotNet.Running;

namespace Microsoft.PowerFx.Performance.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*                                                  
                   1. Build in Release mode

                   2. Then, execute the following commands (from cmd.exe, ELEVATED [required for ETW generation])                       
                        set MSBuildSDKsPath=C:\Program Files\dotnet\sdk\3.1.426\sdks
                        cd <repo root>\src
                        "tests\Microsoft.PowerFx.Performance.Tests\bin\Release\netcoreapp3.1\Microsoft.PowerFx.Performance.Tests.exe"
                                
                     Notice that version 3.1.426 or other versions are obtained with "dotnet --list-sdks"
                     "dotnet new globaljson --sdk-version 3.1.426" shouldn't be needed
                     
                     Results are in 
                       - <repo root>\src\BenchmarkDotNet.Artifacts
                           Microsoft.PowerFx.Performance.Tests.<test class>-<date>-<time>.log
                           Microsoft.PowerFx.Performance.Tests.<test class and name (+opt. N)>-<date>-<time>.etl
                       - <repo root>\src\BenchmarkDotNet.Artifacts\results
                           Microsoft.PowerFx.Performance.Tests.<test class>-report-github.md
                           Microsoft.PowerFx.Performance.Tests.<test class>-report.html
                           Microsoft.PowerFx.Performance.Tests.<test class>-report.csv

                     For .Net 6.0

                     set MSBuildSDKsPath=C:\Program Files\dotnet\sdk\6.0.307\sdks
                     cd <repo root>\src
                     tests\Microsoft.PowerFx.Performance.Tests\bin\Release\net60\Microsoft.PowerFx.Performance.Tests.exe -f *

                     For .Net 7.0

                     set MSBuildSDKsPath=C:\Program Files\dotnet\sdk\7.0.101\sdks
                     cd <repo root>\src
                     tests\Microsoft.PowerFx.Performance.Tests\bin\Release\net70\Microsoft.PowerFx.Performance.Tests.exe -f *
            
             */

            _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
