using DataAccess.EFCore.Tests.Entities;
using DataAccess.EFCore.Tests.ScalarFunctions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccess.EFCore.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private readonly UnitOfWork _target;

        public UnitOfWorkTests()
        {
            var connectionstring = "Data Source=SQLNCLIRDA11;Data Source=TAUCETI\\SQLEXPRESS;Integrated Security=SSPI;Initial Catalog=testdb";

            var optionsBuilder = new DbContextOptionsBuilder<AutoRegisterDbContext<TestDataContext>>();
            optionsBuilder.UseSqlServer(connectionstring);

            var context = new AutoRegisterDbContext<TestDataContext>(optionsBuilder.Options, new TestDataContext());
            _target = new UnitOfWork(context);
        }

        [TestMethod]
        public void Get_WithBasicEntity_GetsEntity()
        {
            var makes = _target.Get<Make>().ToList();

            Assert.IsTrue(makes.Any());
        }

        [TestMethod]
        public void Get_WithForeignKeyedEntity_GetsEntity()
        {
            var models = _target.Get<Model>().ToList();

            Assert.IsTrue(models.Any());
        }

        [TestMethod]
        public void Get_WithNavPropertyEntity_GetsNavValues()
        {
            var models = _target.Get<Model>()
                                .Select(c => c.Make.MakeName)
                                .ToList();

            Assert.IsTrue(models.Any());
        }

        [TestMethod]
        public void Get_WithTwoLevelDeepNavPropertyEntity_GetsNavValues()
        {
            var vehicles = _target.Get<Vehicle>()
                                  .Select(c => new
                                               {
                                                   c.Model.Make.MakeName,
                                                   c.Model.ModelName,
                                                   c.VIN
                                               })
                                  .ToList();

            Assert.IsTrue(vehicles.Any());
        }

        [TestMethod]
        public void Get_WithForeignKeyedCollection_GetsEntities()
        {
            var cars = _target.Get<Person>()
                              .Where(p => p.LastName == "Shull")
                              .Select(p => p.Cars)
                              .ToList();

            Assert.IsTrue(cars.Any());
        }

        [TestMethod]
        public void Where_WithScalarFunction_GetsEntity()
        {
            var people = _target.Get<Person>()
                                .Where(p => Funcs.NumberOfCarsOwned(p.Id) == 2)
                                .ToList();

            Assert.IsTrue(people.Count() == 1);
        }

        [TestMethod]
        public void Select_WithScalarFunction_GetsValues()
        {
            var carsOwned = _target.Get<Person>()
                                   .Select(p => Funcs.NumberOfCarsOwned(p.Id))
                                   .ToList();

            Assert.IsTrue(carsOwned.Count() == 1);
            Assert.IsTrue(carsOwned.First() == 2);
        }

        [TestMethod]
        public void SelectWithGeneric_WithScalarFunction_GetsValues()
        {
            var results = _target.Get<Person>()
                                 .Select(p => new { personId = p.Id, 
                                                    numCarsOwned = Funcs.NumberOfCarsOwned(p.Id) 
                                                  })
                                 .ToList();

            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.First().numCarsOwned == 2);
        }
    }
}