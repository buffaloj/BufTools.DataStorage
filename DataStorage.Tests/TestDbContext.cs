using BufTools.DataStore.EntityFramework;
using BufTools.DataStore.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataStorage.Tests
{
    public class TestDbContext : AutoRegisterDbContext
    {
        public TestDbContext(DbContextOptions options) : base(options)
        {
            RegisterEntities().WithAttribute<StoredDataAttribute>(GetType().Assembly);
            RegisterViews().WithAttribute<StoredViewAttribute>(GetType().Assembly);
            RegisterFunctions().WithAttribute<StoredFunctionAttribute>(GetType().Assembly);
        }
    }
}
