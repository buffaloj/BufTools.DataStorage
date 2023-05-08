using BufTools.DataAnnotations.Schema;
using BufTools.EntityFramework.AutoTypeRegistration;
using Microsoft.EntityFrameworkCore;

namespace DataInjection.EFCore.Tests
{
    public class TestDbContext : AutoRegisterDbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
            RegisterEntities().WithAttribute<EntityAttribute>(GetType().Assembly);
            RegisterViews().WithAttribute<ViewAttribute>(GetType().Assembly);
            RegisterFunctions().WithAttribute<FunctionAttribute>(GetType().Assembly);
        }
    }
}
