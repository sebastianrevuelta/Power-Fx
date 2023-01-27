// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.PowerFx.Core.Utils;

namespace Microsoft.PowerFx.Core.Types
{
    // Implements a red-black tree mapping from string-valued keys to equatable objects.
    // It is an assertable offense for an aggregate to have invalid children.
    internal struct ValueTree : IEquatable<ValueTree>
    {
        private readonly Dictionary<string, EquatableObject> _dic;
        private int? _hash;

        private ValueTree(Dictionary<string, EquatableObject> dic)
        {
            _dic = dic ?? throw new ArgumentNullException(nameof(dic));
            _hash = null;
        }

        internal void AssertValid()
        {
        }

        public static ValueTree Create(IEnumerable<KeyValuePair<string, EquatableObject>> items)
        {
            Contracts.AssertValue(items);

            return new ValueTree(items.ToDictionary(i => i.Key, i => i.Value));
        }

        // Efficient API
        public static ValueTree Create(Dictionary<string, EquatableObject> items)
        {
            return new ValueTree(items);
        }

        public bool IsEmpty => _dic == null || !_dic.Any();

        public int Count => _dic == null ? 0 : _dic.Count;

        public bool Contains(string key) => _dic != null && _dic.ContainsKey(key);

        public bool TryGetValue(string key, out EquatableObject value)
        {
            if (_dic == null)
            {
                value = default;
                return false;
            }

            return _dic.TryGetValue(key, out value);
        }

        public IEnumerable<KeyValuePair<string, EquatableObject>> GetPairs()
        {
            return _dic == null ? Enumerable.Empty<KeyValuePair<string, EquatableObject>>() : _dic.OrderBy(kvp => kvp.Key);
        }

        public ValueTree SetItem(string name, EquatableObject value)
        {
            var d2 = _dic == null ? new Dictionary<string, EquatableObject>() : new Dictionary<string, EquatableObject>(_dic);

            d2[name] = value;
            return new ValueTree(d2);
        }

        // Removes the specified name+value from the tree, and returns a new tree.
        // Sets fMissing if name cannot be found.
        public ValueTree RemoveItem(ref bool fMissing, string name)
        {
            if (fMissing = !_dic.ContainsKey(name))
            {
                return this;
            }            

            var d2 = new Dictionary<string, EquatableObject>(_dic);
            d2.Remove(name);
            return new ValueTree(d2);
        }

        public static bool operator ==(ValueTree tree1, ValueTree tree2) => Equals(tree1, tree2);

        public static bool operator !=(ValueTree tree1, ValueTree tree2) => !(tree1 == tree2);

        public static bool Equals(ValueTree tree1, ValueTree tree2)
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

        public bool Equals(ValueTree other)
        {
            return this == other;
        }

        public override bool Equals(object other)
        {
            if (!(other is ValueTree))
            {
                return false;
            }

            return this == (ValueTree)other;
        }

        public override int GetHashCode()
        {
            if (!_hash.HasValue)
            {
                int hash = 0x79B70F13;

                foreach (KeyValuePair<string, EquatableObject> kvp in GetPairs())
                {
                    hash = Hashing.CombineHash(hash, kvp.Key.GetHashCode());
                    hash = Hashing.CombineHash(hash, kvp.Value.GetHashCode());
                }

                _hash = hash;
            }

            return _hash.Value;
        }
    }
}
