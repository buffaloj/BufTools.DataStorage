using DataAccess.EFCore.Tests.Entities;

namespace DataAccess.EFCore.Tests
{
    public class TestDataContext : AbstractDataContext
    {
        public TestDataContext()
        {
            Include<Make>();
            Include<Model>();
            Include<Vehicle>();
        }
    }
}
