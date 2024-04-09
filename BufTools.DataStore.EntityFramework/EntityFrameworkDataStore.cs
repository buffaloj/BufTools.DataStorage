using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BufTools.DataStore.EntityFramework
{
    /// <inheritdoc/>
    public class EntityFrameworkDataStore<TContext> : IStoreData
        where TContext : DbContext
    {
        private readonly DbContext _dbContext;

        /// <summary>
        /// Constructs an instance that used EntityFrameworkCore
        /// </summary>
        /// <param name="dbContext">An EFCore dbcontext that allows access the database</param>
        /// <exception cref="ArgumentNullException"></exception>
        public EntityFrameworkDataStore(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"{nameof(dbContext)} cannot be null");
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        /// <inheritdoc/>
        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Add(entity);
        }

        /// <inheritdoc/>
        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc/>
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            var dbSet = _dbContext.Set<TEntity>();
            if (_dbContext.Entry(entity).State == EntityState.Detached)
                dbSet.Attach(entity);

            dbSet.Remove(entity);
        }

        /// <inheritdoc/>
        public IRunStoredProcedures<TEntity> Sproc<TEntity>() where TEntity : class
        {
            return new StoredProcRunner<TEntity>(_dbContext.Set<TEntity>());
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> TableFunc<TEntity>(Expression<Func<IQueryable<TEntity>>> expression) where TEntity : class
        {
            return _dbContext.FromExpression(expression);
        }

        /// <inheritdoc/>
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
