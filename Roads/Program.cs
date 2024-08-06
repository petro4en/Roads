using Roads.Models;
using Roads.Services;
using System.Diagnostics;

namespace Roads
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Dictionary<long, Node> nodes;
            //List<long> nodesIds;
            Dictionary<long, Way> ways;
            List<Way> waysList;
            IDataProvider dataProvider = new FileDataProvider();

            //nodes = dataProvider.GetNodes();
            //nodesIds = dataProvider.GetNodesIds();
            //ways = dataProvider.GetWaysDictionary();

            var watch = new Stopwatch();

            watch.Start();
            waysList = dataProvider.GetWays()
                //.Where(w => w.Tags.ContainsKey("highway") && w.Tags["highway"] == "primary" && w.Tags.ContainsKey("name"))
                .Where(w => w.HighwayType == "primary" && w.Name != null)
                .ToList();

            var getWaysDuration = watch.ElapsedMilliseconds;

            //var nodeIds = waysList.SelectMany(w => w.NodeIds).Distinct();
            var nodes = waysList
                            .SelectMany(w => w.NodeIds)
                            .Distinct()
                            .ToDictionary(k => k, v => default(Node?));

            var nodesToDictionaryDuration = watch.ElapsedMilliseconds - getWaysDuration;
            dataProvider.FillNodes(nodes);

            var fillNodesDuration = watch.ElapsedMilliseconds - nodesToDictionaryDuration;
            watch.Stop();

            Console.WriteLine($"getWays finished in\t\t{(double)getWaysDuration/1000} seconds");
            Console.WriteLine($"nodesToDictionary finished in\t\t{(double)nodesToDictionaryDuration / 1000} seconds");
            Console.WriteLine($"fillNodes finished in\t\t{(double)fillNodesDuration / 1000} seconds");
            /*var nodes = ways.SelectMany(w => w.Value.NodeIds).ToArray();
            var distinct = nodes.Distinct().ToArray();*/

            //var isSorted = nodesIds.IsSorted();
        }
    }
}