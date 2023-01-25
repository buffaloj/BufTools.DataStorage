using BufTools.EntityFrameworkCore.DataAutoWiring.Annotations;
using BufTools.EntityFrameworkCore.DataAutoWiring.Exceptions;
using BufTools.EntityFrameworkCore.DataAutoWiring.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BufTools.EntityFrameworkCore.DataAutoWiring.EntityFramework
{
    /// <summary>
    /// A datacontext for EFCore that automatically registers class types with Entity, View, and Function attributes
    /// </summary>
    /// <typeparam name="TContext">The type of <see cref="IDataContext"/> used to specify the data types to register</typeparam>
    public class AutoRegisterDbContext<TContext> : DbContext
        where TContext : IDataContext
    {
        private readonly TContext _context;

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="options">DBContext initialization options</param>
        /// <param name="context">The context object to use</param>
        /// <exception cref="ArgumentNullException">Thrown when a context is not provided</exception>
        public AutoRegisterDbContext(DbContextOptions options, TContext context) : base(options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            var types = _context.GetTypesToRegister();
            foreach (var type in types)
            {
                RegisterIfEntity(modelBuilder, type);
                RegisterIfFunction(modelBuilder, type);
                RegisterIfView(modelBuilder, type);
            }
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
