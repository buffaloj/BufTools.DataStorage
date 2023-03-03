using BufTools.DataStorage.EntityFramework;
using DataInjection.EFCore.Tests.Entities;
using DataInjection.EFCore.Tests.Functions;
using DataInjection.EFCore.Tests.Models;
using DataInjection.EFCore.Tests.Procedures;
using DataInjection.EFCore.Tests.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DataInjection.EFCore.Tests
{
    [TestClass]
    public class UnitOfWorkTests
    {
        private readonly UnitOfWork<TestDataContext> _target;

        public UnitOfWorkTests()
        {
            var connectionstring = "Data Source=host.docker.internal,1433;Initial Catalog=testdb;User ID=SA;Password=change_this_password;TrustServerCertificate=True";

            var optionsBuilder = new DbContextOptionsBuilder<AutoRegisterDbContext<TestDataContext>>();
            optionsBuilder.UseSqlServer(connectionstring);

            var context = new AutoRegisterDbContext<TestDataContext>(optionsBuilder.Options, new TestDataContext());
            _target = new UnitOfWork<TestDataContext>(context);
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
                                                    carsOwned = Funcs.NumberOfCarsOwned(p.Id) 
                                                   })
                                 .ToList();

            Assert.IsTrue(results.Count() == 1);
            Assert.IsTrue(results.First().carsOwned == 2);
        }

        [TestMethod]
        public void TableFunc_WithTableFunction_GetsTableResults()
        {
            var results = _target.TableFunc(() => Funcs.OwnersOfVehicle("12345678901234567"))
                                 .ToList();

            Assert.IsTrue(results.Count() == 1);
        }

        [TestMethod]
        public void Get_WithView_GetsViewResults()
        {
            var makes = _target.Get<VehicleData>().ToList();

            Assert.IsTrue(makes.Any());
        }

        [TestMethod]
        public void Proc_WithValidSproc_Succeeds()
        {
            var owners = _target.Sproc<Owner>().GetOwnersOfVehicle("12345678901234567").ToList();

            Assert.IsTrue(owners.Any());
        }

        [TestMethod]
        public void Insert_WithBasicEntity_InsertsEntity()
        {
            var vehicle = Vehicle.Example();
            vehicle.VIN = MakeUniqueVin();

            _target.Insert(vehicle);
            _target.Save();

            Assert.IsTrue(vehicle.Id > 0);
        }

        private string MakeUniqueVin()
        {
            return DateTime.Now.ToString("mm dd yyyy mm ss HH").Replace(" ", "");
        }

        [TestMethod]
        public void Update_WithBasicEntity_UpdatesEntity()
        {
            var vehicle = _target.Get<Vehicle>().First();
            if (vehicle.ModelId == 1)
                vehicle.ModelId = 2;
            else
                vehicle.ModelId = 1;

            _target.Update(vehicle);
            _target.Save();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Delete_WithBasicEntity_DeletesEntity()
        {
            var vehicle = Vehicle.Example();
            vehicle.VIN = MakeUniqueVin();
            _target.Insert(vehicle);
            _target.Save();

            _target.Delete(vehicle);
            _target.Save();

            Assert.IsTrue(true);
        }
    }
}