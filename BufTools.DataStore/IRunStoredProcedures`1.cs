using System.Linq;

namespace BufTools.DataStore
{
    /// <summary>
    /// An interface to supply params to a stored procedure and then run it
    /// </summary>
    /// <typeparam name="TRet"></typeparam>
    public interface IRunStoredProcedures<TRet> : IRunStoredProcedures
        where TRet : class
    {
        /// <summary>
        /// Used to specify a parameter to be sent when calling a stored procedure
        /// </summary>
        /// <param name="name">The name of the param used in the sproc</param>
        /// <param name="value">The value of the param to be passed to the sproc</param>
        /// <returns>A <see cref="IRunStoredProcedures{TEntity}"/> instance for chaining additional operations</returns>
        IRunStoredProcedures<TRet> WithParam(string name, object value);

        /// <summary>
        /// Executes a stored procedure
        /// </summary>
        /// <param name="name">The name of the stored procedure</param>
        /// <returns>A <see cref="IQueryable"/> for building a more complex query</returns>
        IQueryable<TRet> Run(string name);
    }
}
