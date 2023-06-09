﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using Microsoft.PowerFx.Core.Binding;
using Microsoft.PowerFx.Types;

namespace Microsoft.PowerFx
{
    // Lookup. Called by FirstName node to resolve. 
    internal interface IScope
    {
        // Return null if not found (and then caller can chain to parent)
        FormulaValue Resolve(string name);
    }

    internal class RecordScope : IScope
    {
        public readonly RecordValue _context;

        public RecordScope(RecordValue context)
        {
            _context = context;
        }

        public virtual FormulaValue Resolve(string name)
        {
            return _context.GetField(name);
        }
    }

    internal class ErrorScope : IScope
    {
        public readonly ErrorValue _context;

        public ErrorScope(ErrorValue context)
        {
            _context = context;
        }

        public virtual FormulaValue Resolve(string name)
        {
            return _context;
        }
    }

    internal class UntypedObjectThisRecordScope : IScope
    {
        public readonly FormulaValue _thisRecord;

        public UntypedObjectThisRecordScope(FormulaValue thisItem)
        {
            _thisRecord = thisItem;
        }

        public virtual FormulaValue Resolve(string name)
        {
            if (name == TexlBinding.ThisRecordDefaultName.Value)
            {
                return _thisRecord;
            }

            return null;
        }
    }
}
