// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Microsoft.PowerFx.Performance.Tests
{
    public class PvaPerformance_UT 
    {
        protected PvaPerformance pp;

        public PvaPerformance_UT()             
        {
            pp = new PvaPerformance();
            pp.GlobalSetup();
        }

        [Fact]
        public void PvaPerformance_PvaRecalcEngineConstructorWith10KOptionSets()
        {            
            RecalcEngine engine = pp.PvaRecalcEngineConstructorWith10KOptionSets();
            Assert.NotNull(engine);
        }
    }
}
