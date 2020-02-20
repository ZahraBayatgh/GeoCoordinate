using Microsoft.AspNetCore.Identity;
using Opeqe.Domain.SeedWork;

namespace Opeqe.Identity.Infrastructure.Entities
{
    public class UserClaim : IdentityUserClaim<int>, IAuditableEntity
    {
        public virtual User User { get; set; }
    }
}