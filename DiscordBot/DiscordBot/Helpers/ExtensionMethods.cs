using System;
using System.Collections.Generic;

namespace DiscordBot.Helpers
{
    public static class ExtensionMethods
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var elem in source)
                action(elem);
        }
    }
}
