// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.PowerFx.Core.Utils;

namespace Microsoft.PowerFx.Core.Types
{
    // Implements a red-black tree mapping from string-valued key to DType.
    internal struct TypeTree : IEquatable<TypeTree>, IEnumerable<KeyValuePair<string, DType>>
    {
        private readonly Dictionary<string, DType> _dic;
        private int? _hash;

        private TypeTree(Dictionary<string, DType> dic)
        {
            _dic = dic ?? throw new ArgumentNullException(nameof(dic));
            _hash = null;
        }

        public bool IsEmpty => _dic == null || !_dic.Any();

        public int Count => _dic == null ? 0 : _dic.Count;

        public static bool operator ==(TypeTree tree1, TypeTree tree2) => Equals(tree1, tree2);

        public static bool operator !=(TypeTree tree1, TypeTree tree2) => !(tree1 == tree2);

        public static bool Equals(TypeTree tree1, TypeTree tree2)
        {
            if ((object)tree1 == (object)tree2)
            {
                return true;
            }

            if ((object)tree1 == null || (object)tree2 == null)
            {
                return false;
            }

            if (tree1._dic == tree2._dic || (tree1._dic == null && !tree2._dic.Any()) || (tree2._dic == null && !tree1._dic.Any()))
            {
                return true;
            }

            if (tree1._dic == null || tree2._dic == null)
            {
                return false;
            }

            return tree1._dic.Count == tree2._dic.Count && !tree1._dic.Except(tree2._dic).Any();
        }

        internal void AssertValid()
        {
        }

        public static TypeTree Create(IEnumerable<KeyValuePair<string, DType>> items)
        {
            Contracts.AssertValue(items);
            return new TypeTree(items.GroupBy(i => i.Key).ToDictionary(i => i.Key, i => i.Last().Value));
        }

        public bool Contains(string key) => _dic != null && _dic.ContainsKey(key);

        public bool TryGetValue(string key, out DType value)
        {
            Contracts.AssertValue(key);

            if (_dic == null)
            {
                value = default;
                return false;
            }

            var fRet = _dic.TryGetValue(key, out value);
            value = value ?? DType.Invalid;

            Contracts.Assert(fRet == value.IsValid);

            return fRet;
        }

        public IEnumerable<KeyValuePair<string, DType>> GetPairs()
        {
            return _dic == null ? Enumerable.Empty<KeyValuePair<string, DType>>() : _dic.OrderBy(kvp => kvp.Key);
        }

        internal TypeTree SetItem(string name, DType type, bool useless = false)
        {
            var d2 = _dic == null ? new Dictionary<string, DType>() : new Dictionary<string, DType>(_dic);
            d2[name] = type;
            return new TypeTree(d2);
        }

        // Removes the specified name/field from the tree, and returns a new tree.
        // Sets fMissing if name cannot be found.
        internal TypeTree RemoveItem(ref bool fMissing, string name)
        {
            if (fMissing = !_dic.ContainsKey(name))
            {
                return this;
            }

            var d2 = new Dictionary<string, DType>(_dic);
            d2.Remove(name);
            return new TypeTree(d2);
        }

        public TypeTree RemoveItems(ref bool fAnyMissing, params DName[] rgname)
        {
            Contracts.AssertNonEmpty(rgname);
            Contracts.AssertAllValid(rgname);

            var d2 = new Dictionary<string, DType>(_dic);
            fAnyMissing = false;

            foreach (string name in rgname)
            {
                bool c = d2.ContainsKey(name);
                fAnyMissing |= !c;

                if (c)
                {
                    d2.Remove(name);
                }
            }

            return new TypeTree(d2);
        }

        public bool Equals(TypeTree other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TypeTree))
            {
                return false;
            }

            return this == (TypeTree)obj;
        }

        public override int GetHashCode()
        {
            if (!_hash.HasValue)
            {
                int hash = 0x450C1E25;

                foreach (KeyValuePair<string, DType> kvp in GetPairs())
                {
                    hash = Hashing.CombineHash(hash, kvp.Key.GetHashCode());
                    hash = Hashing.CombineHash(hash, kvp.Value.GetHashCode());
                }

                _hash = hash;
            }

            return _hash.Value;
        }

        IEnumerator<KeyValuePair<string, DType>> IEnumerable<KeyValuePair<string, DType>>.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }
    }
}
