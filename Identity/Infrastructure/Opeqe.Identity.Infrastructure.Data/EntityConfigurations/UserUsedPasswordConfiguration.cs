using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public class UserUsedPasswordConfiguration : IEntityTypeConfiguration<UserUsedPassword>
    {
        public void Configure(EntityTypeBuilder<UserUsedPassword> builder)
        {
            builder.ToTable("AppUserUsedPasswords", CustomIdentityDbContext.DEFAULT_SCHEMA);

            builder.Property(applicationUserUsedPassword => applicationUserUsedPassword.HashedPassword)
                   .HasMaxLength(450)
                   .IsRequired();

            builder.HasOne(applicationUserUsedPassword => applicationUserUsedPassword.User)
                   .WithMany(applicationUser => applicationUser.UserUsedPasswords);
        }
    }
}