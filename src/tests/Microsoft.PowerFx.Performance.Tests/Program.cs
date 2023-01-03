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
                        dotnet new globaljson --sdk-version 3.1.426
                        set MSBuildSDKsPath=C:\Program Files\dotnet\sdk\3.1.426\sdks
                        cd <repo root>\src
                        "tests\Microsoft.PowerFx.Performance.Tests\bin\Release\netcoreapp3.1\Microsoft.PowerFx.Performance.Tests.exe"
            
                     Notice that version 3.1.426 is obtained with "dotnet --list-sdks"
             */

            _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
