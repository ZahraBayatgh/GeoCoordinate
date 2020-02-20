using Location.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Location.Infrastructure.EntityConfigurations
{
    class UserLocationEntityTypeConfiguration
        : IEntityTypeConfiguration<UserLocation>
    {
        public void Configure(EntityTypeBuilder<UserLocation> requestConfiguration)
        {
            requestConfiguration.ToTable("UserLocations", LocationDbContext.DEFAULT_SCHEMA);
            requestConfiguration.HasKey(x => x.Id);
            requestConfiguration.Property(x => x.UserId).IsRequired();
            requestConfiguration.Property(x => x.BaseLatitude).IsRequired();
            requestConfiguration.Property(x => x.BaseLongitude).IsRequired();
            requestConfiguration.Property(x => x.TargetLatitude).IsRequired();
            requestConfiguration.Property(x => x.TargetLongitude).IsRequired();
        }
    }
}
