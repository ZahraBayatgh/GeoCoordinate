using System.Security.Claims;

namespace Opeqe.Infrastructure.Toolkits.UserServices
{
    public interface IUserAccessor
    {
        ClaimsPrincipal User { get; }
        int UserId { get; }
    }
}