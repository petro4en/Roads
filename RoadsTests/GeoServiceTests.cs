using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Roads.Models;
using Roads.Services;
using System.Collections.Generic;
using System.Linq;
using Shouldly;

namespace RoadsTests
{
    [TestFixture]
    public class GeoServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldBeCorrectDistance()
        {
            /*var node1 = new Node { Latitude = 60.00522983981722, Longitude = 30.22479878627223, };
            var node2 = new Node { Latitude = 60.00680105960139, Longitude = 30.212662073187932, };*/
            var expectedDistance = 0.7;

            var nodes = new Dictionary<long, Node?>()
            {
                { 0, new Node { Latitude = 60.00522983981722, Longitude = 30.22479878627223, } },
                { 1, new Node { Latitude = 60.00680105960139, Longitude = 30.212662073187932, } },
            };

            var instance = new GeoCalculator();
            var distance = instance.GetFullDistance(nodes.Keys.ToArray(), nodes);

            distance.ShouldBe(expectedDistance);
        }
    }
}