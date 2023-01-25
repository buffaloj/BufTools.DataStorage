using BufTools.EntityFrameworkCore.DataAutoWiring.EntityFramework;
using BufTools.EntityFrameworkCore.DataAutoWiring.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BufTools.EntityFrameworkCore.DataAutoWiring
{
    /// <summary>
    /// Extension helper
    /// </summary>
    public static class IServiceExtensions
    {
        /// <summary>
        /// Adds a UnitOfWork to the service collection that uses EfCore
        /// </summary>
        /// <typeparam name="TContext">The type of the data context to register types for</typeparam>
        /// <param name="services">The services to add the UnitOfWork to</param>
        /// <param name="optionsAction">A delegate to pass thru the options</param>
        /// <returns>The <see cref="IServiceCollection"/> for chaining</returns>
        public static IServiceCollection AddScopedUnitOfWork<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : IDataContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            services.AddDbContext<AutoRegisterDbContext<TContext>>(optionsAction, ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            return services;
        }
    }
}
