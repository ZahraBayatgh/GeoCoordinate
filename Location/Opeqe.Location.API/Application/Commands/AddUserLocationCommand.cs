using Location.API.Extensions;
using MediatR;
using System;

namespace Microsoft.eShopOnContainers.Services.Location.API.Application.Behaviors.Commands
{
    public class AddUserLocationCommand : IRequest
    {
        public AddUserLocationCommand(string userId, GeoCoordinate baseCoordinates, GeoCoordinate targetCoordinates)
        {
            UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            BaseCoordinates = baseCoordinates ?? throw new ArgumentNullException(nameof(baseCoordinates));
            TargetCoordinates = targetCoordinates ?? throw new ArgumentNullException(nameof(targetCoordinates));
        }

        public string UserId { get; set; }
        public GeoCoordinate BaseCoordinates { get; private set; }
        public GeoCoordinate TargetCoordinates { get; private set; }
    }
}
