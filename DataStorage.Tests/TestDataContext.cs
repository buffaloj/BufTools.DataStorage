using BufTools.DataAnnotations.Schema;
using BufTools.DataStorage;
using DataInjection.EFCore.Tests.Functions;

namespace DataInjection.EFCore.Tests
{
    public class TestDataContext : AbstractDataContext
    {
        public TestDataContext()
        {
            IncludeWithClassAttribute<EntityAttribute>(GetType().Assembly);
            IncludeWithClassAttribute<ViewAttribute>(GetType().Assembly);
            Include(typeof(Funcs));
        }
    }
}
