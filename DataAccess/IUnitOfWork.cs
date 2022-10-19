using System.Linq;

namespace DataAccess
{
    public interface IUnitOfWork
    {
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
    }
}
