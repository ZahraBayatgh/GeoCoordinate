using Opeqe.Identity.Infrastructure.Entities;
using Opeqe.Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Opeqe.Identity.Infrastructure.Settings;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    //public class AppSqlCacheConfiguration : IEntityTypeConfiguration<AppSqlCache>
    //{
    //    private readonly IdentitySettings _sidentitySettings;

    //    public AppSqlCacheConfiguration(IdentitySettings sidentitySettings)
    //    {
    //        _sidentitySettings = sidentitySettings;
    //    }

    //    public void Configure(EntityTypeBuilder<AppSqlCache> builder)
    //    {
    //        // For Microsoft.Extensions.Caching.SqlServer
    //        var cacheOptions = _sidentitySettings.CookieOptions.DistributedSqlServerCacheOptions;
    //        builder.ToTable(cacheOptions.TableName, cacheOptions.SchemaName);
    //        builder.HasIndex(e => e.ExpiresAtTime).HasName("Index_ExpiresAtTime");
    //        builder.Property(e => e.Id).HasMaxLength(449);
    //        builder.Property(e => e.Value).IsRequired();
    //    }
    //}
}