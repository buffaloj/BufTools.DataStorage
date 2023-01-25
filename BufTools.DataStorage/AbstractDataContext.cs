using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BufTools.DataStorage
{
    /// <summary>
    /// This class simplifies registering class types that represent entities, views, functions, and sprocs in a database
    /// by providing reflection methods to register all classes of type T or decorated with an [Attribute]
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

        /// <summary>
        /// Includes all classes with the given class attribute in the assembly
        /// </summary>
        /// <typeparam name="TAttribute">The type of <see cref="Attribute"/> to search for</typeparam>
        /// <param name="assembly">The assembly containing the classes to use</param>
        protected void IncludeWithClassAttribute<TAttribute>(Assembly assembly)
            where TAttribute : Attribute
        {
            var types = GetTypesWithAttribute<TAttribute>(assembly);

            foreach (var type in types)
            {
                if (!_types.Contains(type))
                    _types.Add(type);
            }
        }

        /// <summary>
        /// Includes all classes that contain at least one method with given method attribute
        /// </summary>
        /// <typeparam name="TAttribute">The type of <see cref="Attribute"/> to search for</typeparam>
        /// <param name="assembly">The assembly containing the classes to use</param>
        protected void IncludeClassesWithMethodAttribute<TAttribute>(Assembly assembly)
            where TAttribute : Attribute
        {
            var types = assembly.GetTypes().Where(t => t.GetMethods().Any(m => m.GetCustomAttributes(typeof(TAttribute), true).Any())).ToArray();

            foreach (var type in types)
            {
                if (!_types.Contains(type))
                    _types.Add(type);
            }
        }

        private static Type[] GetTypesWithAttribute<TAttribute>(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any()).ToArray();
        }
    }
}
