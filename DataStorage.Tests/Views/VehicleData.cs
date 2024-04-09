using BufTools.DataStore.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorage.Tests.Views
{
    [StoredView]
    [Table("vehicledata")]
    public class VehicleData
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
    }
}
