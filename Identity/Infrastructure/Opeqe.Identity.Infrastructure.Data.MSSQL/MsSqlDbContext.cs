using Microsoft.EntityFrameworkCore;
using Opeqe.Identity.Infrastructure.Data.Context;

namespace Opeqe.Identity.Infrastructure.Data.MSSQL
{
    public class MsSqlDbContext : CustomIdentityDbContext
    {
        public MsSqlDbContext(DbContextOptions options) : base(options)
        {
        }
    }

}
