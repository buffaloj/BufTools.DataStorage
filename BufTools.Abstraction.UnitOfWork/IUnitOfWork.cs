using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BufTools.Abstraction.UnitOfWork
{
    /// <summary>
    /// Allows interacting with a single database
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Fetches an entity or collection of entities residing in the data source
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being fetched</typeparam>
        /// <returns>>An instance of <see cref="IQueryable"/> for use in building up a more complex query</returns>
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;

        /// <summary>
        /// Inserts a new entity into the data source
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being inserted</typeparam>
        /// <param name="entity">The entity to be inserted</param>
        void Insert<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Updates an existing entity that resides in the data source
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being updated</typeparam>
        /// <param name="entity">The entity to be update</param>
        void Update<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Deletes an existing entity that resides in the data source
        /// </summary>
        /// <typeparam name="TEntity">The type of entity being deleted</typeparam>
        /// <param name="entity">The entity to be deleted</param>
        void Delete<TEntity>(TEntity entity) where TEntity : class;

        /// <summary>
        /// Allows running a table function that resides in the data source
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expression"></param>
        /// <returns>An instance of <see cref="IQueryable"/> for use in building up a more complex query</returns>
        IQueryable<TEntity> TableFunc<TEntity>(Expression<Func<IQueryable<TEntity>>> expression) where TEntity : class;

        /// <summary>
        /// Allows running a stored procedure that resides in the data source
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns>An instance of a <see cref="IProcedure"/> used to access a sproc object</returns>
        IProcedure<TEntity> Sproc<TEntity>() where TEntity : class;

        /// <summary>
        /// Saves to current changes to the database
        /// </summary>
        void Save();

        /// <summary>
        /// Saves to current changes to the database
        /// </summary>
        /// <returns>A <see cref="Task"/> to await</returns>
        Task SaveAsync();
    }
}
