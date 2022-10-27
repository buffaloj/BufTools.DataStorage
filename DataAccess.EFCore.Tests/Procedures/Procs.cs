using DataAccess.EFCore.Tests.Models;

namespace DataAccess.EFCore.Tests.Procedures
{
    public static partial class Procs
    {
        public static IQueryable<Owner> GetOwnersOfVehicle(this IProcedure<Owner> proc,
                                                                string vin)
        {
            return proc.WithParam("@Vin", vin)
                       .Run("[dbo].[get_owners]");
        }
    }
}
