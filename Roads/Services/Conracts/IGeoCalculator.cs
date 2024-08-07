using Roads.Models;
using System.Collections.Generic;

namespace Roads.Services.Conracts
{
    public interface IGeoCalculator
    {
        double GetFullDistance(long[] nodeIds, Dictionary<long, Node> nodes);
    }
}
