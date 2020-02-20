using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Data.Context
{
    public interface IUnitOfWork : IDisposable
    {
        DatabaseFacade Database { get; }
        bool HasActiveTransaction { get; }
        ChangeTracker ChangeTracker { get; }
        IDbContextTransaction GetCurrentTransaction();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class;
        T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible;
        object GetShadowPropertyValue(object entity, string propertyName);

        void ExecuteSqlInterpolatedCommand(FormattableString query);
        void ExecuteSqlRawCommand(string query, params object[] parameters);

        void BeginTransaction();
        void RollbackTransaction();
        void CommitTransaction();

        int SaveChanges(bool acceptAllChangesOnSuccess);
        int SaveChanges();
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
