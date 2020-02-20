using Microsoft.EntityFrameworkCore;
using Opeqe.Identity.Infrastructure.Data.Context;

namespace Opeqe.Identity.Infrastructure.Data.InMemoryDatabase
{
    public class InMemoryDatabaseContext : CustomIdentityDbContext
    {
        public InMemoryDatabaseContext(DbContextOptions options) : base(options)
        {
        }
    }
}