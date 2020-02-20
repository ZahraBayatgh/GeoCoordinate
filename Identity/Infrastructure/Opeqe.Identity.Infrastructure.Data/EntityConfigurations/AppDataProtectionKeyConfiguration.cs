using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public class AppDataProtectionKeyConfiguration : IEntityTypeConfiguration<AppDataProtectionKey>
    {
        public void Configure(EntityTypeBuilder<AppDataProtectionKey> builder)
        {
            builder.ToTable("AppDataProtectionKeys");
            builder.HasIndex(e => e.FriendlyName).IsUnique();
        }
    }
}