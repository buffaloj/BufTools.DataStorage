using DataAccess.Annotations;

namespace DataAccess.EFCore.Tests.Views
{
    [View("vehicledata")]
    public class VehicleData
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
    }
}
