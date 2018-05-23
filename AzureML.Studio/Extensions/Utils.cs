using System;
using System.Collections.Generic;

namespace AzureML.Studio.Extensions
{
    public static class Utils
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }
        }
    }
}
