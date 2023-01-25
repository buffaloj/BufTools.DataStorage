using BufTools.EntityFrameworkCore.DataAutoWiring.Interfaces;
using DataInjection.EFCore.Tests.Models;
using System.Linq;

namespace DataInjection.EFCore.Tests.Procedures
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
