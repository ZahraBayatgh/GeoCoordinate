using Microsoft.AspNetCore.Identity;
using Opeqe.Domain.SeedWork;

namespace Opeqe.Identity.Infrastructure.Entities
{
    public class RoleClaim : IdentityRoleClaim<int>, IAuditableEntity
    {
        public virtual Role Role { get; set; }
    }
}