using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace Opeqe.Infrastructure.Toolkits.UserServices
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly int _userId;

        public UserAccessor(IHttpContextAccessor accessor)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            int.TryParse(accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId);
            _userId = userId;
        }

        public ClaimsPrincipal User => _accessor.HttpContext.User;
        public int UserId => _userId;
    }
}
