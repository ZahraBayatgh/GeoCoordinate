using System;

namespace Location.API.Extensions
{
    public static class GeoCoordinateExtensions
    {
        public static double DistanceTo(this GeoCoordinate baseCoordinates, GeoCoordinate targetCoordinates)
        {
            return DistanceTo(baseCoordinates, targetCoordinates, UnitOfLength.Kilometers);
        }

        public static double DistanceTo(this GeoCoordinate baseCoordinates, GeoCoordinate targetCoordinates, UnitOfLength unitOfLength)
        {
            var baseRad = Math.PI * baseCoordinates.Latitude / 180;
            var targetRad = Math.PI * targetCoordinates.Latitude / 180;
            var theta = baseCoordinates.Longitude - targetCoordinates.Longitude;
            var thetaRad = Math.PI * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return unitOfLength.ConvertFromMiles(dist);
        }
    }
}
