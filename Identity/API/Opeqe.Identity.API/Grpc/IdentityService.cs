using MagicOnion;
using MagicOnion.Server;

namespace Opeqe.Identity.API.Grpc1
{
    public class IdentityService : ServiceBase<IIdentityService>, IIdentityService
    {
        //private readonly ILogger<IdentityService> _logger;

        //public IdentityService(ILogger<IdentityService> logger)
        //{
        //    _logger = logger;
        //}
        // You can use async syntax directly.
        public async UnaryResult<int> LoginAsync(string model)
        {
            //_logger.LogDebug($"login");

            return 0;
        }
    }
}
