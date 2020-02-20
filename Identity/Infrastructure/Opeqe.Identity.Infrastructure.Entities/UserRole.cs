using Microsoft.AspNetCore.Identity;
using Opeqe.Domain.SeedWork;

namespace Opeqe.Identity.Infrastructure.Entities
{
    public class UserRole : IdentityUserRole<int>, IAuditableEntity
    {
        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}