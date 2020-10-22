using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderComparer
{
    public static class Extensions
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, ICollection<TValue>> dict,
            TKey key, Func<ICollection<TValue>> creator, TValue value)
            where TKey : notnull
        {
            if (!dict.ContainsKey(key))
                dict.Add(key, creator.Invoke());

            dict[key].Add(value);
        }
    }
}
