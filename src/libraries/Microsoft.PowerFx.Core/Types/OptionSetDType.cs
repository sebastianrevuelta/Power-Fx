// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.PowerFx.Core.Entities;

namespace Microsoft.PowerFx.Core.Types
{
    internal class OptionSetDType : DType
    {
        public OptionSetDType(IExternalOptionSet externalOptionSet) 
            : base(DKind.OptionSet, TypeTree.Create(externalOptionSet.OptionNames.Select(on => new KeyValuePair<string, DType>(on.Value, DType.String))))
        {             
            OptionSetInfo = externalOptionSet;
            DisplayNameProvider = externalOptionSet.DisplayNameProvider;            
        }
    }
}
