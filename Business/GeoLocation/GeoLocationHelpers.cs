using System;
using System.Linq;
using System.Linq.Expressions;

namespace loconotes.Business.GeoLocation
{
    public static class GeolocationHelpers
    {
        /// <summary>
        /// http://en.wikipedia.org/wiki/Earth_radius
        /// </summary>
        public static double EarthRadiusKilometers { get; } = 6371;

        /// <summary>
        /// http://en.wikipedia.org/wiki/Earth_radius
        /// </summary>
        public static double EarthRadiusMiles { get; } = 3959;

        public enum DistanceType
        {
            Miles,
            Kilometers
        }

        public static double ToRadiansFactor { get; } = Math.PI / 180d;
        public static double ToDegreesFactor { get; } = 180d / Math.PI;
        public static double MinLatDegrees { get; } = -90;
        public static double MaxLatDegrees { get; } = 90;
        public static double MinLonDegrees { get; } = -180;
        public static double MaxLonDegrees { get; } = 180;
        public static double MinLatRadians { get; } = -Math.PI / 2d;
        public static double MaxLatRadians { get; } = Math.PI / 2d;
        public static double MinLonRadians { get; } = -Math.PI;
        public static double MaxLonRadians { get; } = Math.PI;

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        public static double ToRadians(this double degrees)
        {
            return degrees * ToRadiansFactor;
        }

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        public static double ToDegrees(this double radians)
        {
            return radians * ToDegreesFactor;
        }

        /// <summary>
        /// Converts distance (mi. or km.) to radians.
        /// </summary>
        public static double ToDistanceRadians(this double distance, DistanceType distanceType)
        {
            return distance / (distanceType == DistanceType.Miles ? EarthRadiusMiles : EarthRadiusKilometers);
        }

        /// <summary>
        /// Converts radians to distance (mi. or km.).
        /// </summary>
        public static double ToDistance(this double distanceRadians, DistanceType distanceType)
        {
            return distanceRadians * (distanceType == DistanceType.Miles ? EarthRadiusMiles : EarthRadiusKilometers);
        }

        /// <summary>
        /// Computes the bounding coordinates of all points on the surface of the earth that have a great circle distance
        /// to the point represented by the specified location that is less than or equal to the distance argument.
        /// </summary>
        public static GeoCodeRange CalculateGeoCodeRange(
            double latitude,
            double longitude,
            double distance,
            DistanceType distanceType)
        {
            if (latitude < MinLatDegrees || latitude > MaxLatDegrees)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude));
            }
            if (longitude < MinLonDegrees || longitude > MaxLonDegrees)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude));
            }
            if (distance <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(distance));
            }

            double latRadians = latitude.ToRadians();
            double lonRadians = longitude.ToRadians();
            double distanceRadians = distance.ToDistanceRadians(distanceType);

            double minLatRadians = latRadians - distanceRadians;
            double maxLatRadians = latRadians + distanceRadians;

            double minLonRadians, maxLonRadians;
            if (minLatRadians > MinLatRadians && maxLatRadians < MaxLatRadians)
            {
                double deltaLon = Math.Asin(Math.Sin(distanceRadians) / Math.Cos(latRadians));

                minLonRadians = lonRadians - deltaLon;
                if (minLonRadians < MinLonRadians)
                {
                    minLonRadians += 2d * Math.PI;
                }

                maxLonRadians = lonRadians + deltaLon;
                if (maxLonRadians > MaxLonRadians)
                {
                    maxLonRadians -= 2d * Math.PI;
                }
            }
            else
            {
                // a pole is within the distance
                minLatRadians = Math.Max(minLatRadians, MinLatRadians);
                maxLatRadians = Math.Min(maxLatRadians, MaxLatRadians);
                minLonRadians = MinLonRadians;
                maxLonRadians = MaxLonRadians;
            }

            return new GeoCodeRange
            {
                MinimumLatitude = minLatRadians.ToDegrees(),
                MaximumLatitude = maxLatRadians.ToDegrees(),
                MinimumLongitude = minLonRadians.ToDegrees(),
                MaximumLongitude = maxLonRadians.ToDegrees()
            };
        }

        /// <summary>
        /// Returns true if the <see cref="GeoCodeRange"/> spans the 180th meridian.
        /// </summary>
        public static bool IncludesMeridian180(this GeoCodeRange geoCodeRange)
        {
            if (geoCodeRange == null)
            {
                throw new ArgumentNullException(nameof(geoCodeRange));
            }

            return geoCodeRange.MinimumLongitude > geoCodeRange.MaximumLongitude;
        }

        /// <summary>
        /// Filters a sequence of <see cref="IGeoCode"/> items based on a specified <see cref="GeoCodeRange"/>.
        /// Useful for database queries for better performance.
        /// </summary>
        public static IQueryable<T> WhereInGeoCodeRange<T>(
            this IQueryable<T> query,
            GeoCodeRange geoCodeRange)
            where T : IGeoCode
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (geoCodeRange == null)
            {
                throw new ArgumentNullException(nameof(geoCodeRange));
            }

            // The logic for bounding coordinates is different depending on whether geoCodeRange includes the 180th meridian.
            Expression<Func<IGeoCode, bool>> geoCodeRangePredicate = geoCodeRange.IncludesMeridian180()
                ? (Expression<Func<IGeoCode, bool>>)(x =>
                    (x.LatitudeGeoCode >= geoCodeRange.MinimumLatitude && x.LatitudeGeoCode <= geoCodeRange.MaximumLatitude)
                    && (x.LongitudeGeoCode >= geoCodeRange.MinimumLongitude || x.LongitudeGeoCode <= geoCodeRange.MaximumLongitude)
                )
                : x =>
                    (x.LatitudeGeoCode >= geoCodeRange.MinimumLatitude && x.LatitudeGeoCode <= geoCodeRange.MaximumLatitude)
                    && (x.LongitudeGeoCode >= geoCodeRange.MinimumLongitude && x.LongitudeGeoCode <= geoCodeRange.MaximumLongitude);

            return ((IQueryable<IGeoCode>)query)
                .Where(geoCodeRangePredicate)
                .Cast<T>();
        }

        /// <summary>
        /// Calculates the distance between two locations.
        /// </summary>
        public static double CalculateDistance(
            double latitude1,
            double longitude1,
            double latitude2,
            double longitude2,
            DistanceType distanceType)
        {
            double lat1Radians = latitude1.ToRadians();
            double lon1Radians = longitude1.ToRadians();
            double lat2Radians = latitude2.ToRadians();
            double lon2Radians = longitude2.ToRadians();
            double earthRadius = distanceType == DistanceType.Miles
                ? EarthRadiusMiles
                : EarthRadiusKilometers;

            return Math.Acos(
                Math.Sin(lat1Radians) * Math.Sin(lat2Radians) +
                Math.Cos(lat1Radians) * Math.Cos(lat2Radians) * Math.Cos(lon1Radians - lon2Radians)
            ) * earthRadius;
        }
    }
}
