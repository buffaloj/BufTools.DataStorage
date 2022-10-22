using System;
using System.Collections.Generic;

namespace DataAccess
{
    public abstract class AbstractDataContext : IDataContext
    {
        private readonly IList<Type> _types = new List<Type>();

        public IEnumerable<Type> GetTypesToRegister()
        {
            return _types;
        }

        protected void Include<T>()
            where T : class
        {
            if (!_types.Contains(typeof(T)))
                _types.Add(typeof(T));
        }
    }
}
