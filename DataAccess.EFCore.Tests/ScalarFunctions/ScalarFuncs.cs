using DataAccess.Annotations;

namespace DataAccess.EFCore.Tests.ScalarFunctions
{
    public static partial class Funcs
    {
        [Function("number_of_cars_owned", "dbo")]
        public static int NumberOfCarsOwned(int personId)
            => throw new NotSupportedException();
    }
}
