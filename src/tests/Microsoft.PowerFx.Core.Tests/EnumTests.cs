// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.PowerFx.Core.Types;
using Microsoft.PowerFx.Core.Types.Enums;
using Xunit;

namespace Microsoft.PowerFx.Core.Tests
{
    public class EnumTests
    {
        [Fact]
        public void ValidateDefaultEnums()
        {
            foreach (KeyValuePair<string, string> kvp in EnumStoreBuilder.DefaultEnums)
            {            
                DType.TryParse(kvp.Value, out DType dType);
                DType dType2 = EnumStoreBuilder.DefaultEnums2[kvp.Key];

                Assert.True(dType == dType2, $"Not Equal:\r\n{dType}\r\n{dType2}");                
            }
        }
    }
}
