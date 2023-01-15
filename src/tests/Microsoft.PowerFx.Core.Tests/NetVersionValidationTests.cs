// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Reflection;
using System.Runtime.Versioning;
using Xunit;

namespace Microsoft.PowerFx.Core.Tests
{
    public class NetVersionValidationTests
    {
        [Fact]
        public void ValidateDotNetVersion()
        {
            string netVersion = typeof(string).Assembly.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName
                    ?? AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
            netVersion = netVersion.Substring(netVersion.Length - 4);

#if NETCOREAPP3_1
            Assert.Equal("v2.1", netVersion);
#elif NET6_0 
            Assert.Equal("v6.0", netVersion);
#elif NET7_0
            Assert.Equal("v7.0", netVersion);
#else
#error Invalid .Net version    
#endif
        }
    }
}
