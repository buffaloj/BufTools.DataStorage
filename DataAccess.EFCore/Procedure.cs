using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.EFCore
{
    /// <summary>
    /// A class that runs a stored procedure in a database
    /// </summary>
    /// <typeparam name="TEntity">The type of entity returned by the stored procedure</typeparam>
    public class Procedure<TEntity> : IProcedure<TEntity>
         where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private List<SqlParameter> _params = new List<SqlParameter>();

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="dbSet">The dbSet to run the SPROC on</param>
        public Procedure(DbSet<TEntity> dbSet)
        {
            _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
        }

        /// <inheritdoc/>
        public IProcedure<TEntity> WithParam(string name, object value)
        {
            _params.Add(new SqlParameter(name, value));
            return this;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Run(string name)
        {
            var paramNames = _params.Select(p => p.ParameterName);
            var sql = $"EXEC {name} {string.Join(", ", paramNames)}";
            return _dbSet.FromSqlRaw(sql, _params.ToArray());
        }
    }
}
