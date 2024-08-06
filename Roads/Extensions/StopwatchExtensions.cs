using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Roads.Extensions
{
    public static class StopwatchExtensions
    {
        public static void FixTimeAndRestart(this Stopwatch stopwatch, string mesuredEventName, ILogger logger)
        {
            stopwatch.FixTime(mesuredEventName, logger);
            stopwatch.Restart();
        }

        public static void FixTimeAndStop(this Stopwatch stopwatch, string mesuredEventName, ILogger logger)
        {
            stopwatch.FixTime(mesuredEventName, logger);
            stopwatch.Stop();
        }

        public static void FixTime(this Stopwatch stopwatch, string mesuredEventName, ILogger logger)
        {
            var elapsed = stopwatch.ElapsedMilliseconds;
            logger.LogInformation($"{mesuredEventName} finished in\t\t{(double)elapsed / 1000} seconds");
        }
    }
}
