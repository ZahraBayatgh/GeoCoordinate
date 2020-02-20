using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Services.Contracts;
using Opeqe.Infrastructure.Data.Context;
using Opeqe.Infrastructure.Toolkits.GuardToolkit;
using System.Security.Claims;

namespace Opeqe.Identity.Infrastructure.Services
{
    public class ApplicationRoleStore :
        RoleStore<Role, Data.Context.CustomIdentityDbContext, int, UserRole, RoleClaim>,
        IApplicationRoleStore
    {
        private readonly IUnitOfWork _uow;
        private readonly IdentityErrorDescriber _describer;

        public ApplicationRoleStore(
            IUnitOfWork uow,
            IdentityErrorDescriber describer)
            : base((Data.Context.CustomIdentityDbContext)uow, describer)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _describer = describer;
            _describer.CheckArgumentIsNull(nameof(_describer));
        }


        #region BaseClass

        protected override RoleClaim CreateRoleClaim(Role role, Claim claim)
        {
            return new RoleClaim
            {
                RoleId = role.Id,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            };
        }

        #endregion

        #region CustomMethods

        #endregion
    }
}