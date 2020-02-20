using Location.API.Extensions;

namespace Location.API.ViewModels
{
    public class GeoCoordinateViewModel
    {
        public GeoCoordinate BaseCoordinates { get; set; }
        public GeoCoordinate TargetCoordinates { get; set; }
    }
}
