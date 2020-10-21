using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FolderComparer.Tools
{
    public class DictionaryKeyArray : IEquatable<DictionaryKeyArray>
    {
        public readonly Byte[] ByteHash;
        public DictionaryKeyArray(Byte[] array) => ByteHash = array;

        public bool Equals(DictionaryKeyArray other)
        {
            if (other is null)
                return false;

            if (ByteHash.Length != other.ByteHash.Length)
                return false;

            if (ByteHash.First() != other.ByteHash.First() || ByteHash.Last() != other.ByteHash.Last())
                return false;

            return ByteHash.SequenceEqual(other.ByteHash);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int p = 16777619;
                int hash = (int)2166136261;

                for (int i = 0; i < ByteHash.Length; i++)
                    hash = (hash ^ ByteHash[i]) * p;

                hash += hash << 13;
                hash ^= hash >> 7;
                hash += hash << 3;
                hash ^= hash >> 17;
                hash += hash << 5;

                return hash;
            }
        }
    }
}
