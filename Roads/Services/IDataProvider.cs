using Roads.Models;

namespace Roads.Services
{
    public interface IDataProvider
    {
        void FillNodes(Dictionary<long, Node?> nodes);
        Dictionary<long, Node> GetNodes();
        
        List<long> GetNodesIds();
        IEnumerable<Way> GetWays();
        Dictionary<long, Way> GetWaysDictionary();
    }
}
