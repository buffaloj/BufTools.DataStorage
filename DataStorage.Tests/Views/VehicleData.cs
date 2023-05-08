using BufTools.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataInjection.EFCore.Tests.Views
{
    [View]
    [Table("vehicledata")]
    public class VehicleData
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
    }
}
