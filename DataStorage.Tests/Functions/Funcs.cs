using BufTools.DataStore.Schema;
using DataStorage.Tests.Models;
using System;
using System.Linq;

namespace DataStorage.Tests.Functions
{
    public static partial class Funcs
    {
        [StoredFunction("number_of_cars_owned", "dbo")]
        public static int NumberOfCarsOwned(int personId)
            => throw new NotSupportedException();

        [StoredFunction("owners_of_vehicle", "dbo")]
        public static IQueryable<Owner> OwnersOfVehicle(string vin)
            => throw new NotSupportedException();
    }
}
