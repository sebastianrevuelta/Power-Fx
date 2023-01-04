// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerFx.Connectors.Net6.Tests.Helpers
{
    public static class Extensions
    {
#if NETCOREAPP3_1
        public static async Task<string> ReadAsStringAsync(this HttpContent httpContent, CancellationToken cancellationToken)
        {
            return await httpContent.ReadAsStringAsync();
        }    
#endif
    }
}
