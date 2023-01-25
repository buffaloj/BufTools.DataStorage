using BufTools.EntityFrameworkCore.DataAutoWiring.Annotations;
using DataInjection.EFCore.Tests.Models;
using System;
using System.Linq;

namespace DataInjection.EFCore.Tests.Functions
{
    public static partial class Funcs
    {
        [Function("number_of_cars_owned", "dbo")]
        public static int NumberOfCarsOwned(int personId)
            => throw new NotSupportedException();

        [Function("owners_of_vehicle", "dbo")]
        public static IQueryable<Owner> OwnersOfVehicle(string vin)
            => throw new NotSupportedException();
    }
}
