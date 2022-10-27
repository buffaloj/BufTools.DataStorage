using DataAccess.Annotations;
using DataAccess.EFCore.Tests.Models;

namespace DataAccess.EFCore.Tests.Functions
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
