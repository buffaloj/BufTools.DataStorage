using System.Linq;

namespace DataAccess
{
    public interface IProcedure<TEntity>
        where TEntity : class
    {
        IProcedure<TEntity> WithParam(string name, object value);
        IQueryable<TEntity> Run(string name);
    }
}
