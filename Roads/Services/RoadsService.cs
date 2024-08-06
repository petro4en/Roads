using Microsoft.Extensions.Logging;
using Roads.Extensions;
using Roads.Models;
using Roads.Services.Conracts;
using System.Diagnostics;

namespace Roads.Services
{
    public sealed class RoadsService : IRoadsService
    {
        private readonly IDataProvider dataProvider;
        private readonly ILogger<RoadsService> logger;

        public RoadsService(IDataProvider dataProvider, ILogger<RoadsService> logger)
        {
            this.dataProvider = dataProvider;
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
            watch.FixTimeAndStop(nameof(dataProvider.FillNodes), logger);
        }
    }
}
