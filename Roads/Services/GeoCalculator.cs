using Roads.Services.Conracts;
using System;

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

        /// <summary> Prime vertical </summary>
        public double N(double phi)
        {
            var esin = eccentricity * Math.Sin(phi);
            return equatorialRadius / Math.Sqrt(1 - esin * esin);
        }

        /// <summary> Meridional radius of curvature </summary>
        public double M(double phi)
        {
            var n = N(phi);
            var tripleN = n * n * n;
            return (1 - eccentricity * eccentricity) / (equatorialRadius * equatorialRadius) * tripleN;
        }

        /// <summary>
        /// Length calculating method for middle latitudes for short lines by Gauss
        /// </summary>
        /// <param name="phi"> latitude </param>
        /// <param name="lambda"> longitude </param>
        /// <returns></returns>
        public double GaussLengthForMidLatitudeShortLines(double phi1, double lambda1, double phi2, double lambda2)
        {
            var midPhi = (phi1 + phi2) / 2;
            var deltaPhi = phi2 - phi1;
            var deltaLambda = lambda2 - lambda1;
            var n = N(midPhi);

            var sqrtItem1 = Math.Sin(deltaLambda / 2 * Math.Cos(midPhi));
            var sqrtItem2 = Math.Cos(deltaLambda / 2) * Math.Sin(M(midPhi) * deltaPhi / 2 * n);

            return 2 * n * Math.Asin(Math.Sqrt(sqrtItem1 * sqrtItem1 + sqrtItem2 * sqrtItem2));
        }
    }
}
