using System.Linq;

namespace BufTools.Abstraction.UnitOfWork
{
    /// <summary>
    /// An interface to supply params to a stored procedure and then run it
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IProcedure<TEntity> : IProcedure
        where TEntity : class
    {
        /// <summary>
        /// Used to specify a parameter to be sent when calling a stored procedure
        /// </summary>
        /// <param name="name">The name of the param used in the sproc</param>
        /// <param name="value">The value of the param to be passed to the sproc</param>
        /// <returns>A <see cref="IProcedure{TEntity}"/> instance for chaining additional operations</returns>
        IProcedure<TEntity> WithParam(string name, object value);

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="name">The name of the stored procedure</param>
        /// <returns>A <see cref="IQueryable"/> for building a more complex query</returns>
        IQueryable<TEntity> Run(string name);
    }
}
