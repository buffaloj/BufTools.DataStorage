using DataAccess.EFCore.Tests.Entities;
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
    }
}