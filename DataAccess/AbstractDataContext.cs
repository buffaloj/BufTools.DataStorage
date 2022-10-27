using System;
using System.Collections.Generic;

namespace DataAccess
{
    /// <summary>
    /// This class simplifies registering class types that represent entities, views, functions, and sprocs in a database
    /// </summary>
    public abstract class AbstractDataContext : IDataContext
    {
        private readonly IList<Type> _types = new List<Type>();

        /// <inheritdoc/>
        public IEnumerable<Type> GetTypesToRegister()
        {
            return _types;
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <typeparam name="T">Type type of the class</typeparam>
        protected void Include<T>()
            where T : class
        {
            if (!_types.Contains(typeof(T)))
                _types.Add(typeof(T));
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <param name="type">Type type of the class</param>
        protected void Include(Type type)
        {
            if (!_types.Contains(type))
                _types.Add(type);
        }
    }
}
