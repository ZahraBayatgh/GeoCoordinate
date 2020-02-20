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
    /// Repository
    /// </summary>
    public partial interface IRepository<T> where T : DTO
    {

        IMongoCollection<T> Collection { get; }

        IMongoDatabase Database { get; }

        /// <summary>
        /// Get async entity by identifier 
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        Task<T> GetByIdAsync(string id);

        /// <summary>
        /// Async Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<T> InsertAsync(T entity);

        /// <summary>
        /// Async Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<IEnumerable<T>> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// Async Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<T> UpdateAsync(T entity);
        /// <summary>
        /// Async Update or Insert entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpsertAsync(T entity);

        /// <summary>
        /// Async Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<IEnumerable<T>> UpdateAsync(IEnumerable<T> entities);
        /// <summary>
        /// Async Update or Insert entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> UpsertAsync(IEnumerable<T> entities);


        /// Async Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<T> DeleteAsync(T entity);

        /// <summary>
        /// Async Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        Task<IEnumerable<T>> DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Async determines whether a list contains any elements
        /// </summary>
        /// <returns></returns>
        Task<bool> AnyAsync();

        /// <summary>
        /// Async determines whether any element of a list satisfies a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Async returns the number of elements in the specified sequence
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        /// <summary>
        /// Async returns the number of elements in the specified sequence that satisfies a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<long> CountAsync(Expression<Func<T, bool>> where);

        /// <summary>
        /// Gets a table
        /// </summary>
        IMongoQueryable<T> Table { get; }

        /// <summary>
        /// Get collection by filter definitions
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        IList<T> FindByFilterDefinition(FilterDefinition<T> query);
    }
}