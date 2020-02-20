using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(userClaim => userClaim.User)
                   .WithMany(user => user.Claims)
                   .HasForeignKey(userClaim => userClaim.UserId);

            builder.ToTable("AppUserClaims", CustomIdentityDbContext.DEFAULT_SCHEMA);
        }
    }
}