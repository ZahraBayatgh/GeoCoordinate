using MediatR;
using System.Collections.Generic;

namespace Opeqe.Location.API.Application.Queries
{
    public class UserLocationQuery : IRequest<IEnumerable<GeoCoordinateDto>>
    {
        public UserLocationQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; private set; }
    }
}


