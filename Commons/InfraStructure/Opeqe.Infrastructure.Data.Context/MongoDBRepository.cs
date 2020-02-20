using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Opeqe.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Opeqe.Infrastructure.Data.Context
{
    /// <summary>
    /// MongoDB repository
    /// </summary>
    public partial class MongoDBRepository<T> : IRepository<T> where T : DTO
    {
        #region Fields

        /// <summary>
        /// Gets the collection
        /// </summary>
        protected IMongoCollection<T> _collection;
        public IMongoCollection<T> Collection => _collection;

        /// <summary>
        /// Mongo Database
        /// </summary>
        protected IMongoDatabase _database;
        public IMongoDatabase Database => _database;

        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        public MongoDBRepository(MongoSettings mongoSettings)
        {
            string mongoConnectionString = mongoSettings.MongoDbConnectionString;
            var client = new MongoClient(mongoConnectionString);
            var databaseName = mongoSettings.MongoDbDatabase;/*new MongoUrl(mongoConnectionString).DatabaseName*/;
            _database = client.GetDatabase(databaseName);
            _collection =
            _database.GetCollection<T>(typeof(T).Name + "s");
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get entity by identifier async
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual Task<T> GetByIdAsync(string id)
        {
            return this._collection.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Async Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> InsertAsync(T entity)
        {
            await this._collection.InsertOneAsync(entity);
            return entity;
        }

        /// <summary>
        /// Async Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> InsertAsync(IEnumerable<T> entities)
        {
            await this._collection.InsertManyAsync(entities);
            return entities;
        }

        /// <summary>
        /// Async Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> UpdateAsync(T entity)
        {
            await this._collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = false });
            return entity;
        }

        /// <summary>
        /// Async Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> UpsertAsync(T entity)
        {
            await this._collection.ReplaceOneAsync(x => x.Id == entity.Id, entity, new UpdateOptions() { IsUpsert = true });
            return entity;
        }

        /// <summary>
        /// Async Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpdateAsync(entity);
            }
            return entities;
        }

        /// <summary>
        /// Async Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> UpsertAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await UpsertAsync(entity);
            }
            return entities;
        }

        /// <summary>
        /// Async Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual async Task<T> DeleteAsync(T entity)
        {
            await this._collection.DeleteOneAsync(e => e.Id == entity.Id);
            return entity;
        }

        /// <summary>
        /// Async Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                await DeleteAsync(entity);
            }
            return entities;
        }

        /// <summary>
        /// Determines whether a list contains any elements
        /// </summary>
        /// <returns></returns>
        public virtual bool Any()
        {
            return this._collection.AsQueryable().Any();
        }

        /// <summary>
        /// Determines whether any element of a list satisfies a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual bool Any(Expression<Func<T, bool>> where)
        {
            return this._collection.Find(where).Any();
        }

        /// <summary>
        /// Async determines whether a list contains any elements
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync()
        {
            return await this._collection.AsQueryable().AnyAsync();
        }

        /// <summary>
        /// Async determines whether any element of a list satisfies a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
        {
            return await this._collection.Find(where).AnyAsync();
        }

        /// <summary>
        /// Returns the number of elements in the specified sequence.
        /// </summary>
        /// <returns></returns>
        public virtual long Count()
        {
            return this._collection.CountDocuments(new BsonDocument());
        }

        /// <summary>
        /// Returns the number of elements in the specified sequence that satisfies a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual long Count(Expression<Func<T, bool>> where)
        {
            return this._collection.CountDocuments(where);
        }

        /// <summary>
        /// Async returns the number of elements in the specified sequence
        /// </summary>
        /// <returns></returns>
        public virtual async Task<long> CountAsync()
        {
            return await this._collection.CountDocumentsAsync(new BsonDocument());
        }

        /// <summary>
        /// Async returns the number of elements in the specified sequence that satisfies a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual async Task<long> CountAsync(Expression<Func<T, bool>> where)
        {
            return await this._collection.CountDocumentsAsync(where);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IMongoQueryable<T> Table => this._collection.AsQueryable();

        /// <summary>
        /// Get collection by filter definitions
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual IList<T> FindByFilterDefinition(FilterDefinition<T> query)
        {
            return this._collection.Find(query).ToList();
        }

        #endregion

    }
    public class MongoSettings
    {
        public MongoSettings(string mongoDbConnectionString, string mongoDbDatabase)
        {
            MongoDbConnectionString = mongoDbConnectionString;
            MongoDbDatabase = mongoDbDatabase;
        }

        public string MongoDbConnectionString { get; set; }
        public string MongoDbDatabase { get; set; }
    }
}
