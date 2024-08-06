using System.Globalization;

namespace Roads.Extensions
{
    public static class StringExtensions
    {
        public static double ToDouble(this string source)
            => double.Parse(source, NumberStyles.Any, CultureInfo.InvariantCulture);
    }
}
