using BufTools.DataAnnotations.Schema;
using BufTools.EntityFramework.AutoTypeRegistration.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BufTools.EntityFramework.AutoTypeRegistration
{
    /// <summary>
    /// A datacontext for EFCore that automatically registers class types with Entity, View, and Function attributes
    /// </summary>
    public class AutoRegisterDbContext : DbContext
    {
        private readonly IList<Registrar> _funcs = new List<Registrar>();
        private readonly IList<Registrar> _entities = new List<Registrar>();
        private readonly IList<Registrar> _views = new List<Registrar>();

        /// <summary>
        /// Constructs an instance
        /// </summary>
        /// <param name="options">DBContext initialization options</param>
        public AutoRegisterDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Registers type(s) as an Entity (in a relational DB, this maps to a DB table)
        /// </summary>
        /// <returns>A <see cref="Registrar"/> to specify the type(s) you want to register</returns>
        protected Registrar RegisterEntities()
        {
            _entities.Add(new Registrar());
            return _entities.Last();
        }

        /// <summary>
        /// Registers type(s) as an View (in a relational DB, this maps to a DB view)
        /// </summary>
        /// <returns>A <see cref="Registrar"/> to specify the type(s) you want to register</returns>
        protected Registrar RegisterViews()
        {
            _views.Add(new Registrar());
            return _views.Last();
        }

        /// <summary>
        /// Registers class type(s) that contains are least one static function (in a relational DB, this maps to a DB scalar function)
        /// </summary>
        /// <returns>A <see cref="Registrar"/> to specify the type(s) you want to register</returns>
        protected Registrar RegisterFunctions()
        {
            _funcs.Add(new Registrar());
            return _funcs.Last();
        }

        /// <summary>
        /// Method that registers data type with EntityFrameworkCore
        /// </summary>
        /// <param name="modelBuilder">The builder to register with</param>
        /// <exception cref="ArgumentNullException">Thrown if modelBuilder is null</exception>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            Register(modelBuilder, _entities, RegisterAsEntity);
            Register(modelBuilder, _views, RegisterAsView);
            Register(modelBuilder, _funcs, RegisterAsFunc);
        }

        private void Register(ModelBuilder modelBuilder, IEnumerable<Registrar> registrars, Action<ModelBuilder, Type> registrationMethod)
        {
            foreach (var registrar in registrars) 
                foreach (var type in registrar.Types)
                    registrationMethod.Invoke(modelBuilder, type);
        }

        private void RegisterAsEntity(ModelBuilder modelBuilder, Type type)
        {
            var entity = modelBuilder.Entity(type);

            //if (!string.IsNullOrWhiteSpace(attribute.TableName))
            //    entity.ToTable(attribute.TableName, attribute.Schema);
            //else
            //    entity.HasNoKey();

            var props = type.GetProperties()
                            .Where(info => Attribute.IsDefined(info, typeof(CompositeKeyAttribute)))
                            .Select(p => p.Name)
                            .ToArray();
            if (props.Any())
            {
                entity.HasKey(props);
                return;
            }

            props = type.GetProperties()
                            .Where(info => Attribute.IsDefined(info, typeof(KeyAttribute)))
                            .Select(p => p.Name)
                            .ToArray();
            if (props.Any())
            {
                entity.HasKey(props);
                return;
            }

            entity.HasNoKey();
        }

        private void RegisterAsView(ModelBuilder modelBuilder, Type type)
        {
            var entity = modelBuilder.Entity(type);
            entity.HasNoKey();

            //if (!string.IsNullOrWhiteSpace(attribute.ViewName))
            //    entity.ToView(attribute.ViewName, attribute.Schema);
        }

        private void RegisterAsFunc(ModelBuilder modelBuilder, Type type)
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
    }
}
