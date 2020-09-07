using System;
using System.Collections.Generic;

namespace FolderComparer
{
    public static class IReadOnlyExtensions
    {
        public static void ForEach<T>(this IReadOnlyList<T> list, Action<T> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            foreach (T item in list)
            {
                action.Invoke(item);
            }
        }
}
}