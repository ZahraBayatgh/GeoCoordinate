using Opeqe.Identity.Infrastructure.Services.Contracts;
using System.Security.Claims;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class SecurityTrimmingService : ISecurityTrimmingService
    {
        //private readonly HttpContext _httpContext;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly IMvcActionsDiscoveryService _mvcActionsDiscoveryService;
        ////private readonly Data.Context.CustomIdentityDbContext _CustomIdentityDbContext;

        //public SecurityTrimmingService(HttpContext httpContext,
        //                               IHttpContextAccessor httpContextAccessor,
        //                               IMvcActionsDiscoveryService mvcActionsDiscoveryService
        //    //,CustomIdentityDbContext customIdentityDbContext
        //    )
        //{
        //    _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
        //    _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        //    _mvcActionsDiscoveryService = mvcActionsDiscoveryService ?? throw new ArgumentNullException(nameof(mvcActionsDiscoveryService));
        //    //_CustomIdentityDbContext = customIdentityDbContext ?? throw new ArgumentNullException(nameof(customIdentityDbContext));
        //}

        public bool CanCurrentUserAccess(string area, string controller, string action)
        {
            //return _httpContext != null && CanUserAccess(_httpContext.User, area, controller, action);
            return true;
        }

        public bool CanUserAccess(ClaimsPrincipal user, string area, string controller, string action)
        {
            return true;
            //var currentClaimValue = $"{area}:{controller}:{action}";
            //var securedControllerActions = _mvcActionsDiscoveryService.GetAllSecuredControllerActionsWithPolicy(ConstantPolicies.DynamicPermission);
            //if (!securedControllerActions.SelectMany(x => x.MvcActions).Any(x => x.ActionId == currentClaimValue))
            //{
            //    throw new KeyNotFoundException($"The `secured` area={area}/controller={controller}/action={action} with `ConstantPolicies.DynamicPermission` policy not found. Please check you have entered the area/controller/action names correctly and also it's decorated with the correct security policy.");
            //}

            //if (!user.Identity.IsAuthenticated)
            //{
            //    return false;
            //}
            ////var u = _CustomIdentityDbContext.RoleClaims.Any(x=>x.ClaimType== ConstantPolicies.DynamicPermissionClaimType 
            ////&& x.ClaimValue==currentClaimValue
            ////)
            //if (user.IsInRole(ConstantRoles.Admin) || user.HasClaim(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" && c.Value == "Admin"))
            //{
            //    // Admin users have access to all of the pages.
            //    return true;
            //}

            //// Check for dynamic permissions
            //// A user gets its permissions claims from the `ApplicationClaimsPrincipalFactory` class automatically and it includes the role claims too.
            //return user.HasClaim(claim => claim.Type == ConstantPolicies.DynamicPermissionClaimType &&
            //                              claim.Value == currentClaimValue);
            ////return _CustomIdentityDbContext.RoleClaims.Any(x => x.ClaimType == ConstantPolicies.DynamicPermissionClaimType
            ////&& x.ClaimValue == currentClaimValue
            ////);
        }
    }
}