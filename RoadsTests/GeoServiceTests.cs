using NUnit.Framework;
using Roads.Models;
using Roads.Services;
using Roads.Services.Conracts;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadsTests
{
    [TestFixture]
    public class GeoServiceTests
    {
        private readonly IGeoCalculator instance = new GeoCalculator();

        [Test]
        public void ShouldBeCorrectDistance()
        {
            // Given
            var expectedDistance = 0.7;

            var nodes = new Dictionary<long, Node>()
            {
                { 0, new Node { Latitude = 60.00522983981722, Longitude = 30.22479878627223, } },
                { 1, new Node { Latitude = 60.00680105960139, Longitude = 30.212662073187932, } },
            };

            // When
            var distance = instance.GetFullDistance(nodes.Keys.ToArray(), nodes);

            // Than
            var deltaPercents = Math.Abs(expectedDistance - distance) / expectedDistance * 100;

            deltaPercents.ShouldBeLessThan(10);
        }
    }
}