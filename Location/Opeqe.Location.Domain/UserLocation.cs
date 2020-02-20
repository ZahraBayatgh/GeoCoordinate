using Location.Domain.SeedWork;
using System;

namespace Location.Domain
{
    public class UserLocation : Entity
    {
        public UserLocation(string userId, double baseLatitude, double baseLongitude, double targetLatitude, double targetLongitude)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            BaseLatitude = baseLatitude;
            BaseLongitude = baseLongitude;
            TargetLatitude = targetLatitude;
            TargetLongitude = targetLongitude;
        }

        public string UserId { get; set; }
        public double BaseLatitude { get; set; }
        public double BaseLongitude { get; set; }
        public double TargetLatitude { get; set; }
        public double TargetLongitude { get; set; }
    }
}
