namespace Microsoft.eShopOnContainers.Services.Location.API.Application.Behaviors.Commands
{
    using global::Location.Domain;
    using global::Location.Infrastructure;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AddUserLocationCommandHandler : IRequestHandler<AddUserLocationCommand>
    {
        private readonly LocationDbContext _context;
        private readonly ILogger<AddUserLocationCommandHandler> _logger;

        public AddUserLocationCommandHandler(LocationDbContext context,
            ILogger<AddUserLocationCommandHandler> logger)
        {
            _context = context;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Unit> Handle(AddUserLocationCommand request, CancellationToken cancellationToken)
        {

            _context.UserLocations.Add(new UserLocation(
               request.UserId,
               request.BaseCoordinates.Latitude,
               request.BaseCoordinates.Longitude,
               request.TargetCoordinates.Latitude,
               request.TargetCoordinates.Longitude
               ));

            _context.SaveChanges();

            return Unit.Value;
        }

    }
}
