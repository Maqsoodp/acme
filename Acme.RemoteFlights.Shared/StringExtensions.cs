using System;

namespace Acme.RemoteFlights.Shared
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string stringToCheck, StringComparison stringComparison)
        {
            return source?.IndexOf(stringToCheck, stringComparison) >= 0;
        }
    }
}
