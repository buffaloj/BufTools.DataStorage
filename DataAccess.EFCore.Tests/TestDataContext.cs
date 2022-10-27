using DataAccess.EFCore.Tests.Functions;
using DataAccess.Annotations;

namespace DataAccess.EFCore.Tests
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
