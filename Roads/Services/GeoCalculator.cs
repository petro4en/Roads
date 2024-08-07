using Roads.Extensions;
using Roads.Models;
using Roads.Services.Conracts;
using System;
using System.Collections.Generic;

namespace Roads.Services
{
    public sealed class GeoCalculator : IGeoCalculator
    {
        // Earth constant parameters
        /// <summary> e </summary>
        const double eccentricity = 0.016708617;
        /// <summary> a </summary>
        const double equatorialRadius = 6378.1370;
        /// <summary> b </summary>
        const double polarRadius = 6356.7523;
        /// <summary> R </summary>
        const double radius = 6371.009;

        public double GetFullDistance(long[] nodeIds, Dictionary<long, Node> nodes)
        {
            if (nodeIds == null || nodeIds.Length < 2)
            {
                return 0;
            }

            double distance = 0;

            for (int i = 1; i < nodeIds.Length; i++)
            {
                var node1 = nodes[nodeIds[i - 1]];
                var node2 = nodes[nodeIds[i]];

                distance += GaussLengthForMidLatitudeShortLines(
                    node1.Latitude,
                    node1.Longitude,
                    node2.Latitude,
                    node2.Longitude);
            }

            return distance;
        }

        /// <summary>
        /// Length calculating method for middle latitudes for short lines by Gauss
        /// </summary>
        /// <param name="phi1"> first point latitude </param>
        /// <param name="lambda1"> first point longitude </param>
        /// <param name="phi2"> second point latitude </param>
        /// <param name="lambda2"> second point longitude </param>
        /// <returns></returns>
        private double GaussLengthForMidLatitudeShortLines(double phi1, double lambda1, double phi2, double lambda2)
        {
            var midPhi = (phi1 + phi2) / 2;
            var deltaPhi = phi2 - phi1;
            var deltaLambda = lambda2 - lambda1;
            var n = N(midPhi);

            var sqrtItem1 = Sin(deltaLambda / 2) * Cos(midPhi);
            var sqrtItem2 = Cos(deltaLambda / 2) * Sin(M(midPhi) * deltaPhi / (2 * n));

            return 2 * n * Asin(Math.Sqrt(sqrtItem1 * sqrtItem1 + sqrtItem2 * sqrtItem2));
        }

        /// <summary>
        /// Tunnel length calculating method
        /// </summary>
        /// <param name="phi1"> first point latitude </param>
        /// <param name="lambda1"> first point longitude </param>
        /// <param name="phi2"> second point latitude </param>
        /// <param name="lambda2"> second point longitude </param>
        /// <returns></returns>
        private double TunnelLength(double phi1, double lambda1, double phi2, double lambda2)
        {
            var midPhi = (phi1 + phi1) / 2;
            var deltaPhi = phi2 - phi1;
            var deltaLambda = lambda2 - lambda1;

            var sqrtItem1 = Sin(deltaLambda / 2) * Cos(midPhi);
            var sqrtItem2 = Cos(deltaLambda / 2) * Sin(deltaPhi / 2);

            return 2 * radius * Math.Sqrt(sqrtItem1 * sqrtItem1 + sqrtItem2 * sqrtItem2);
        }

        /// <summary> Prime vertical </summary>
        private double N(double phi)
        {
            var esin = eccentricity * Sin(phi);
            return equatorialRadius / Math.Sqrt(1 - esin * esin);
        }

        /// <summary> Meridional radius of curvature </summary>
        private double M(double phi)
        {
            var n = N(phi);
            var tripleN = n * n * n;
            return (1 - eccentricity * eccentricity) / (equatorialRadius * equatorialRadius) * tripleN;
        } 

        private static double Sin(double degreeAngle) => Math.Sin(degreeAngle.ToRadians());
        private static double Cos(double degreeAngle) => Math.Cos(degreeAngle.ToRadians());
        private static double Asin(double value) => Math.Asin(value);
    }
}
