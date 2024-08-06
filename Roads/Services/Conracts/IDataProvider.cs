using Roads.Models;

namespace Roads.Services.Conracts
{
    public interface IDataProvider
    {
        IEnumerable<Way> GetWays();

        void FillNodes(Dictionary<long, Node?> nodes);
    }
}
