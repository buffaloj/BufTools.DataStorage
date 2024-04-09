using BufTools.DataStore;
using DataStorage.Tests.Models;
using System.Linq;

namespace DataStorage.Tests.Procedures
{
    public static partial class Procs
    {
        public static IQueryable<Owner> GetOwnersOfVehicle(this IRunStoredProcedures<Owner> procRunner,
                                                                string vin)
        {
            return procRunner
                .WithParam("@Vin", vin)
                .Run("[dbo].[get_owners]");
        }
    }
}
