using Microsoft.EntityFrameworkCore;

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
    }
}
