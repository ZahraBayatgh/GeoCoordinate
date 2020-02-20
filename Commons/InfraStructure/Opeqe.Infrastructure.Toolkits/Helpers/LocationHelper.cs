using System;

namespace Opeqe.Infrastructure.Toolkits.Helpers
{
    public struct GeoLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public struct GeoArea
    {
        public GeoLocation TopLeft { set; get; }
        public GeoLocation TopRight { set; get; }
        public GeoLocation BottomLeft { set; get; }
        public GeoLocation BottomRight { set; get; }
    }
    public class LocationHelper
    {
        public static GeoArea FindAreaAtDistanceFrom(GeoLocation point, double distanceKilometers)
        {
            GeoLocation top = FindPointAtDistanceFrom(point, DegreesToRadians(0), distanceKilometers);
            GeoLocation right = FindPointAtDistanceFrom(point, DegreesToRadians(90), distanceKilometers);
            GeoLocation bottom = FindPointAtDistanceFrom(point, DegreesToRadians(180), distanceKilometers);
            GeoLocation left = FindPointAtDistanceFrom(point, DegreesToRadians(270), distanceKilometers);
            GeoArea area = new GeoArea
            {
                TopLeft = new GeoLocation { Latitude = top.Latitude, Longitude = left.Longitude },
                TopRight = new GeoLocation { Latitude = top.Latitude, Longitude = right.Longitude },
                BottomRight = new GeoLocation { Latitude = bottom.Latitude, Longitude = right.Longitude },
                BottomLeft = new GeoLocation { Latitude = bottom.Latitude, Longitude = left.Longitude },
            };
            return area;
        }
        public static GeoLocation FindPointAtDistanceFrom(GeoLocation startPoint, double initialBearingRadians, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            double distRatio = distanceKilometres / radiusEarthKilometres;
            double distRatioSine = Math.Sin(distRatio);
            double distRatioCosine = Math.Cos(distRatio);

            double startLatRad = DegreesToRadians(startPoint.Latitude);
            double startLonRad = DegreesToRadians(startPoint.Longitude);

            double startLatCos = Math.Cos(startLatRad);
            double startLatSin = Math.Sin(startLatRad);

            double endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));

            double endLonRads = startLonRad
                    + Math.Atan2(
                            Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
                            distRatioCosine - startLatSin * Math.Sin(endLatRads));

            return new GeoLocation
            {
                Latitude = RadiansToDegrees(endLatRads),
                Longitude = RadiansToDegrees(endLonRads)
            };
        }

        public static double DegreesToRadians(double degrees)
        {
            const double degToRadFactor = Math.PI / 180;
            return degrees * degToRadFactor;
        }

        public static double RadiansToDegrees(double radians)
        {
            const double radToDegFactor = 180 / Math.PI;
            return radians * radToDegFactor;
        }


        public static double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double distanceInMiles(double lon1d, double lat1d, double lon2d, double lat2d)
        {
            double lon1 = ToRadians(lon1d);
            double lat1 = ToRadians(lat1d);
            double lon2 = ToRadians(lon2d);
            double lat2 = ToRadians(lat2d);

            double deltaLon = lon2 - lon1;
            double c = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon));
            double earthRadius = 3958.76;
            double distInMiles = earthRadius * c;

            return distInMiles;
        }
    }
}
