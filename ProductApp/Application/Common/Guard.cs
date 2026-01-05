using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Application.Common
{
    internal static class Guard
    {
        public static void NotNull<T>(T? value, string paramName)
            where T : class
        {
            if (value is null) throw new ArgumentNullException(paramName);
        }

        public static void NotNullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"{paramName} is required.", paramName);
        }

        public static void GreaterThan(decimal value, decimal minExclusive, string paramName)
        {
            if (value <= minExclusive)
                throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} must be greater than {minExclusive}.");
        }

        public static void GreaterThan(int value, int minExclusive, string paramName)
        {
            if (value <= minExclusive)
                throw new ArgumentOutOfRangeException(paramName, value, $"{paramName} must be greater than {minExclusive}.");
        }
    }
}
