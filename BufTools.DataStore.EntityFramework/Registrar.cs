using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BufTools.DataStore.EntityFramework
{
    /// <summary>
    /// A class that lets you specify one or more class types to use for registration and includes reflection options to find the types you want to register
    /// </summary>
    public class Registrar
    {
        internal IList<Type> Types { get; private set; } = new List<Type>();
        internal Type AttributeType { get; private set; }

        /// <summary>
        /// Registers a specific type
        /// </summary>
        /// <param name="type">The class type to register</param>
        public void WithType(Type type)
        {
            if (!Types.Contains(type))
                Types.Add(type);
        }

        /// <summary>
        /// Registers a specific type
        /// </summary>
        public void WithType<T>()
            where T : class
        {
            WithType(typeof(T));
        }

        /// <summary>
        /// Registers all class types that have the given class attribute or contain a method with the given method attribute
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for</param>
        /// <param name="assembly">The assembly to search in</param>
        public void WithAttribute(Type attributeType, Assembly assembly)
        {
            var classTypes = assembly.GetTypes().Where(t => t.GetCustomAttributes(attributeType, true).Any()).ToArray();
            foreach (var type in classTypes)
                WithType(type);

            var methodTypes = assembly.GetTypes().Where(t => t.GetMethods().Any(m => m.GetCustomAttributes(attributeType, true).Any())).ToArray();
            foreach (var type in methodTypes)
                WithType(type);

            AttributeType = attributeType;
        }

        /// <summary>
        /// Registers all class types that have the given class attribute or contain a method with the given method attribute
        /// </summary>
        /// <typeparam name="T">The type of attribute to search for</typeparam>
        /// <param name="assembly">The assembly to search in</param>
        public void WithAttribute<T>(Assembly assembly)
            where T : Attribute
        {
            WithAttribute(typeof(T), assembly);
        }
    }
}
