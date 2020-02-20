using Microsoft.EntityFrameworkCore;
using Opeqe.Identity.Infrastructure.Settings;

namespace Opeqe.Identity.Infrastructure.Data.EntityConfigurations
{
    public static class IdentityMappings
    {
        public static void AddCustomIdentityMappings(this ModelBuilder modelBuilder, IdentitySettings sidentitySettings)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IdentityMappings).Assembly);

        }
    }
}
