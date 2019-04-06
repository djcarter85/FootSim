namespace FootSim.Grid
{
    using System.Collections.Generic;

    public static class StringExtensions
    {
        public static string Join(this IEnumerable<string> source, string separator = null) => string.Join(separator, source);
    }
}