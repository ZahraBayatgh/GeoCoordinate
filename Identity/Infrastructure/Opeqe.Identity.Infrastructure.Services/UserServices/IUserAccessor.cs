using System.Security.Claims;

namespace Opeqe.Identity.Infrastructure.Services.UserServices
{
    public interface IUserAccessor
    {
        ClaimsPrincipal User { get; }
        int UserId { get; }
    }
}