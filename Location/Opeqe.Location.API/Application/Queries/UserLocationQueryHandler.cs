using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opeqe.Location.API.Application.Queries
{
    public class UserLocationQueryHandler : IRequestHandler<UserLocationQuery, IEnumerable<GeoCoordinateDto>>
    {
        private readonly IConfiguration _configuration;

        public UserLocationQueryHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<IEnumerable<GeoCoordinateDto>> Handle(UserLocationQuery request, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_configuration["ConnectionString"]))
            {
                connection.Open();

                var userId = request.UserId.ToString();

                var result = await connection.QueryAsync<GeoCoordinateDto>(
                   @"SELECT  BaseLatitude, BaseLongitude, TargetLatitude, TargetLongitude
                     FROM [Identity].location.UserLocations
                     WHERE UserId=@userId"
                        , new { userId }
                    );

                return result;
            }

        }
    }

    public class GeoCoordinateDto
    {
        public double BaseLatitude { get; set; }
        public double BaseLongitude { get; set; }
        public double TargetLatitude { get; set; }
        public double TargetLongitude { get; set; }

    }
}


