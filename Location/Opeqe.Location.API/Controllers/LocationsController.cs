using Location.API.Extensions;
using Location.API.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.Services.Location.API.Application.Behaviors.Commands;
using Microsoft.Extensions.Logging;
using Opeqe.Location.API.Application.Queries;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Location.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/[controller]")]
    public class LocationsController : ControllerBase
    {
        private readonly IMediator _mediatR;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(IMediator mediatR, ILogger<LocationsController> logger)
        {
            _mediatR = mediatR;
            _logger = logger;
        }
        public async Task<IEnumerable<GeoCoordinateDto>> Get()
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));

            var request = new UserLocationQuery(userId);

            var result =await _mediatR.Send(request);

            return result;
        }

        [HttpPost]
        public async Task<double> DistanceTo([FromBody]GeoCoordinateViewModel geoCoordinateViewModel)
        {
            var userId = this.User.FindFirstValue("UserId");

            var result = geoCoordinateViewModel.BaseCoordinates.DistanceTo(geoCoordinateViewModel.TargetCoordinates);

            var request = new AddUserLocationCommand(userId, geoCoordinateViewModel.BaseCoordinates, geoCoordinateViewModel.TargetCoordinates);

           await _mediatR.Send(request);

            return result;
        }
    }
}
