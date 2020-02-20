using Microsoft.AspNetCore.Identity;
using Opeqe.Domain.SeedWork;

namespace Opeqe.Identity.Infrastructure.Entities
{
    public class UserLogin : IdentityUserLogin<int>, IAuditableEntity
    {
        public virtual User User { get; set; }
    }
}