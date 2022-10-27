using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;

        void Insert<TEntity>(TEntity entity) where TEntity : class;

        IQueryable<TEntity> TableFunc<TEntity>(Expression<Func<IQueryable<TEntity>>> expression) where TEntity : class;

        IProcedure<TEntity> Proc<TEntity>() where TEntity : class;
    }
}
