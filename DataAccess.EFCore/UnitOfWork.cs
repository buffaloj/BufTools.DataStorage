using System;
using System.Linq;

namespace DataAccess.EFCore
{
    public class UnitOfWork : IUnitOfWork
    {
        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }
    }
}
