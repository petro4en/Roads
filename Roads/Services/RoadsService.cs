using Microsoft.Extensions.Logging;
using Roads.Extensions;
using Roads.Models;
using Roads.Services.Conracts;
using System.Diagnostics;
using System.Linq;

namespace Roads.Services
{
    public sealed class RoadsService : IRoadsService
    {
        private readonly IDataProvider dataProvider;
        private readonly IGeoCalculator geoCalculator;
        private readonly ILogger<RoadsService> logger;

        public RoadsService(
            IDataProvider dataProvider,
            IGeoCalculator geoCalculator,
            ILogger<RoadsService> logger)
        {
            this.dataProvider = dataProvider;
            this.geoCalculator = geoCalculator;
            this.logger = logger;
        }

        public void ProcessRoads()
        {
            var watch = Stopwatch.StartNew();

            var ways = dataProvider.GetWays()
                        //.Where(w => w.Tags.ContainsKey("highway") && w.Tags["highway"] == "primary" && w.Tags.ContainsKey("name"))
                        .Where(w => w.HighwayType == "primary" && w.Name != null)
                        .ToArray();

            watch.FixTimeAndRestart(nameof(dataProvider.GetWays), logger);

            //var nodeIds = waysList.SelectMany(w => w.NodeIds).Distinct();
            var nodes = ways
                            .SelectMany(w => w.NodeIds)
                            .Distinct()
                            .ToDictionary(k => k, v => default(Node?));

            watch.FixTimeAndRestart("nodesToDictionary", logger);
            
            dataProvider.FillNodes(nodes);
            watch.FixTimeAndRestart(nameof(dataProvider.FillNodes), logger);

            //var distances = new Road[ways.Length];

            for (int i = 0; i < ways.Length; i++)
            {
                ways[i].Distance = geoCalculator.GetFullDistance(ways[i].NodeIds, nodes);
            }

            watch.FixTimeAndStop("Calculate distances", logger);
        }
    }
}
