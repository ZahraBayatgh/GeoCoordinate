using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.HasOne(roleClaim => roleClaim.Role)
                   .WithMany(role => role.Claims)
                   .HasForeignKey(roleClaim => roleClaim.RoleId);

            builder.ToTable("AppRoleClaims", CustomIdentityDbContext.DEFAULT_SCHEMA);
        }
    }
}