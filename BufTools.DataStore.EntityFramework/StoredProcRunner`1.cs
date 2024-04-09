using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BufTools.DataStore.EntityFramework
{
    /// <summary>
    /// A class that runs a stored procedure in a database
    /// </summary>
    /// <typeparam name="TRet">The type of entity returned by the stored procedure</typeparam>
    public class StoredProcRunner<TRet> : IRunStoredProcedures<TRet>
         where TRet : class
    {
        private readonly DbSet<TRet> _dbSet;
        private List<SqlParameter> _params = new List<SqlParameter>();

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="dbSet">The dbSet to run the SPROC on</param>
        public StoredProcRunner(DbSet<TRet> dbSet)
        {
            _dbSet = dbSet ?? throw new ArgumentNullException(nameof(dbSet));
        }

        /// <inheritdoc/>
        public IRunStoredProcedures<TRet> WithParam(string name, object value)
        {
            _params.Add(new SqlParameter(name, value));
            return this;
        }

        /// <inheritdoc/>
        public IQueryable<TRet> Run(string name)
        {
            var paramNames = _params.Select(p => p.ParameterName);
            var sql = $"EXEC {name} {string.Join(", ", paramNames)}";
            return _dbSet.FromSqlRaw(sql, _params.ToArray());
        }
    }
}
