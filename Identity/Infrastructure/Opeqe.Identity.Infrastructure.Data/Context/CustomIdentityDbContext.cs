namespace Opeqe.Identity.Infrastructure.Data.Context
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Opeqe.Identity.Infrastructure.Data.EntityConfigurations;
    using Opeqe.Identity.Infrastructure.Entities;
    using Opeqe.Identity.Infrastructure.Settings;
    using Opeqe.Infrastructure.Toolkits.AuditableEntity;
    using Opeqe.Infrastructure.Toolkits.EFCoreToolkit;
    using Opeqe.Infrastructure.Toolkits.GuardToolkit;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;

    public partial class CustomIdentityDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, Opeqe.Infrastructure.Data.Context.IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "dbo";
        private IDbContextTransaction _transaction;

        // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
        public CustomIdentityDbContext(DbContextOptions options)
            : base(options) { }
        public virtual DbSet<AppLoginData> AppLoginDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // it should be placed here, otherwise it will rewrite the following settings!
            base.OnModelCreating(builder);

            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            // Adds all of the ASP.NET Core Identity related mappings at once.
            var sidentitySettings = this.GetService<IdentitySettings>();
            builder.AddCustomIdentityMappings(sidentitySettings);

            // Custom application mappings
            builder.SetDecimalPrecision();
            builder.AddDateTimeUtcKindConverter();

            // This should be placed here, at the end.
            builder.AddAuditableShadowProperties();
        }

        public virtual DbSet<AppLogItem> AppLogItems { get; set; }
        public virtual DbSet<AppSqlCache> AppSqlCache { get; set; }
        public virtual DbSet<AppDataProtectionKey> AppDataProtectionKeys { get; set; }
        private IDbContextTransaction _currentTransaction;

        // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`

        #region BaseClass

        public bool HasActiveTransaction => _currentTransaction != null;

        public void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void BeginTransaction()
        {
            _currentTransaction = Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void CommitTransaction()
        {
            if (_currentTransaction == null) throw new ArgumentNullException(nameof(_currentTransaction));

            try
            {
                SaveChanges();
                _currentTransaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public override void Dispose()
        {
            _currentTransaction?.Dispose();
            base.Dispose();
        }

        public void ExecuteSqlInterpolatedCommand(FormattableString query)
        {
            Database.ExecuteSqlInterpolated(query);
        }

        public void ExecuteSqlRawCommand(string query, params object[] parameters)
        {
            Database.ExecuteSqlRaw(query, parameters);
        }

        public T GetShadowPropertyValue<T>(object entity, string propertyName) where T : IConvertible
        {
            var value = this.Entry(entity).Property(propertyName).CurrentValue;
            return value != null
                ? (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture)
                : default;
        }

        public object GetShadowPropertyValue(object entity, string propertyName)
        {
            return this.Entry(entity).Property(propertyName).CurrentValue;
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Update(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges(acceptAllChangesOnSuccess);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges(); //NOTE: changeTracker.Entries<T>() will call it automatically.

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChanges();
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            ChangeTracker.DetectChanges();

            beforeSaveTriggers();

            ChangeTracker.AutoDetectChangesEnabled = false; // for performance reasons, to avoid calling DetectChanges() again.
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            ChangeTracker.AutoDetectChangesEnabled = true;
            return result;
        }

        private void beforeSaveTriggers()
        {
            validateEntities();
            setShadowProperties();
        }

        private void setShadowProperties()
        {
            // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
            var httpContextAccessor = this.GetService<IHttpContextAccessor>();
            ChangeTracker.SetAuditableEntityPropertyValues(httpContextAccessor);
        }

        private void validateEntities()
        {
            var errors = this.GetValidationErrors();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                // we can't use constructor injection anymore, because we are using the `AddDbContextPool<>`
                var loggerFactory = this.GetService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<CustomIdentityDbContext>();
                logger.LogError(errors);
                throw new InvalidOperationException(errors);
            }
        }

        public IDbContextTransaction GetCurrentTransaction()
        {
            return _currentTransaction;
        }

        #endregion

    }

}

