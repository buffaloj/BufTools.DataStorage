using DataAccess.EFCore.Tests.Entities;
using DataAccess.EFCore.Tests.ScalarFunctions;

namespace DataAccess.EFCore.Tests
{
    public class TestDataContext : AbstractDataContext
    {
        public TestDataContext()
        {
            Include<Make>();
            Include<Model>();
            Include<Vehicle>();
            Include<Person>();
            Include<PersonCar>();
            Include(typeof(Funcs));
        }
    }
}
