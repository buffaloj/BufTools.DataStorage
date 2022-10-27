using DataAccess.EFCore.Tests.Entities;
using DataAccess.EFCore.Tests.Models;
using DataAccess.EFCore.Tests.Functions;
using DataAccess.EFCore.Tests.Views;

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
            Include<Owner>();
            Include<VehicleData>();
            Include(typeof(Funcs));
        }
    }
}
