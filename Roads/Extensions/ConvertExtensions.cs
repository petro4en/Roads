using System;
using System.Globalization;

namespace Roads.Extensions
{
    public static class ConvertExtensions
    {
        public static double ToDouble(this string source)
            => double.Parse(source, NumberStyles.Any, CultureInfo.InvariantCulture);

        public static double ToRadians(this double degreeAngle)
            => Math.PI * degreeAngle / 180.0;
    }
}
