using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AppUsers", CustomIdentityDbContext.DEFAULT_SCHEMA);
        }
    }
}