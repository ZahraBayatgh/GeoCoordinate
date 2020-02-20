using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Data.Context;
using Opeqe.Identity.Infrastructure.Entities;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public class AppLogItemConfiguration : IEntityTypeConfiguration<AppLogItem>
    {
        public void Configure(EntityTypeBuilder<AppLogItem> builder)
        {
            builder.ToTable("AppLogItems", CustomIdentityDbContext.DEFAULT_SCHEMA);
        }
    }
}