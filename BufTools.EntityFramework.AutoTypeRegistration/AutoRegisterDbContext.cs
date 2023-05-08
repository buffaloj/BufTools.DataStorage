using BufTools.DataAnnotations.Schema;
using BufTools.DataStorage.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BufTools.DataStorage.EntityFramework
{
    /// <summary>
    /// A datacontext for EFCore that automatically registers class types with Entity, View, and Function attributes
    /// </summary>
    public class AutoRegisterDbContext : DbContext
    {
        private readonly IList<Type> _funcs = new List<Type>();

        private readonly IList<Type> _entities = new List<Type>();
        private readonly IList<Type> _views = new List<Type>();

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="options">DBContext initialization options</param>
        public AutoRegisterDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <typeparam name="T">Type type of the class</typeparam>
        protected void IncludeEntity<T>()
            where T : class
        {
            if (!_entities.Contains(typeof(T)))
                _entities.Add(typeof(T));
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <param name="type">Type type of the class</param>
        protected void IncludeEntity(Type type)
        {
            if (!_entities.Contains(type))
                _entities.Add(type);
        }



        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <typeparam name="T">Type type of the class</typeparam>
        protected void IncludeView<T>()
            where T : class
        {
            if (!_views.Contains(typeof(T)))
                _views.Add(typeof(T));
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <param name="type">Type type of the class</param>
        protected void IncludeView(Type type)
        {
            if (!_views.Contains(type))
                _views.Add(type);
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <typeparam name="T">Type type of the class</typeparam>
        protected void IncludeFunctionClass<T>()
            where T : class
        {
            if (!_funcs.Contains(typeof(T)))
                _funcs.Add(typeof(T));
        }

        /// <summary>
        /// Includes a specific class type for use with the database
        /// </summary>
        /// <param name="type">Type type of the class</param>
        protected void IncludeFunctionClass(Type type)
        {
            if (!_funcs.Contains(type))
                _funcs.Add(type);
        }





        /// <summary>
        /// Includes all classes with the given class attribute in the assembly
        /// </summary>
        /// <typeparam name="TAttribute">The type of <see cref="Attribute"/> to search for</typeparam>
        /// <param name="assembly">The assembly containing the classes to use</param>
        protected void IncludeEntitiesWithClassAttribute<TAttribute>(Assembly assembly)
            where TAttribute : Attribute
        {
            var types = GetTypesWithAttribute<TAttribute>(assembly);

            foreach (var type in types)
            {
                if (!_entities.Contains(type))
                    _entities.Add(type);
            }
        }

        /// <summary>
        /// Includes all classes with the given class attribute in the assembly
        /// </summary>
        /// <typeparam name="TAttribute">The type of <see cref="Attribute"/> to search for</typeparam>
        /// <param name="assembly">The assembly containing the classes to use</param>
        protected void IncludeViewsWithClassAttribute<TAttribute>(Assembly assembly)
            where TAttribute : Attribute
        {
            var types = GetTypesWithAttribute<TAttribute>(assembly);

            foreach (var type in types)
            {
                if (!_views.Contains(type))
                    _views.Add(type);
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
                if (!_funcs.Contains(type))
                    _funcs.Add(type);
            }
        }

        private static Type[] GetTypesWithAttribute<TAttribute>(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(TAttribute), true).Any()).ToArray();
        }

        /// <summary>
        /// Method that registers data with EntityFrameworkCore
        /// </summary>
        /// <param name="modelBuilder">The builder to register with</param>
        /// <exception cref="ArgumentNullException">Thrown if modelBuilder is null</exception>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            foreach (var type in _entities)
                RegisterIfEntity(modelBuilder, type);

            foreach (var type in _views)
                RegisterIfFunction(modelBuilder, type);

            foreach (var type in _funcs)
                RegisterIfView(modelBuilder, type);
        }

        private void RegisterIfEntity(ModelBuilder modelBuilder, Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(EntityAttribute), true).FirstOrDefault() as EntityAttribute;
            if (attribute != null)
            {
                var entity = modelBuilder.Entity(type);

                if (!string.IsNullOrWhiteSpace(attribute.TableName))
                    entity.ToTable(attribute.TableName, attribute.Schema);
                else
                    entity.HasNoKey();

                var props = type.GetProperties()
                                .Where(info => Attribute.IsDefined(info, typeof(CompositeKeyAttribute)))
                                .Select(p => p.Name)
                                .ToArray();

                if (props.Any())
                    entity.HasKey(props);
            }
        }

        private void RegisterIfFunction(ModelBuilder modelBuilder, Type type)
        {
            var methods = type.GetMethods().Where(info => Attribute.IsDefined(info, typeof(FunctionAttribute)));

            if (methods.Any(p => !p.IsStatic))
                throw new NonStaticFunctionException(methods.Where(p => !p.IsStatic).Select(p => p.Name));

            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttributes(typeof(FunctionAttribute), true).First() as FunctionAttribute;
                modelBuilder.HasDbFunction(method).HasName(attribute?.FunctionName ?? "")
                                                  .HasSchema(attribute?.Schema ?? "");
            }
        }

        private void RegisterIfView(ModelBuilder modelBuilder, Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(ViewAttribute), true).FirstOrDefault() as ViewAttribute;
            if (attribute != null)
            {
                var entity = modelBuilder.Entity(type);
                entity.HasNoKey();

                if (!string.IsNullOrWhiteSpace(attribute.ViewName))
                    entity.ToView(attribute.ViewName, attribute.Schema);
            }
        }
    }
}
