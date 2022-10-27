using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"{nameof(dbContext)} cannot be null");
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return _dbContext.Set<TEntity>();
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            _dbContext.Add(entity);
        }

        public IProcedure<TEntity> Proc<TEntity>() where TEntity : class
        {
            return new Procedure<TEntity>(_dbContext.Set<TEntity>());
        }

        public IQueryable<TEntity> TableFunc<TEntity>(Expression<Func<IQueryable<TEntity>>> expression) where TEntity : class
        {
            return _dbContext.FromExpression(expression);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public Task SaveAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
