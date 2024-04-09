using BufTools.DataStore.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataStorage.Tests.Entities
{
    [StoredData]
    [Table("person_car")]
    public class PersonCar
    {
        [CompositeKey]
        [Column("person_id")]
        public int PersonId { get; set; }

        [CompositeKey]
        [Column("vehicle_id")]
        public int VehicleId { get; set; }

        //[JsonIgnore]
        //[ForeignKey(nameof(PersonId))]
        //public virtual Person Person { get; set; }

        //[JsonIgnore]
        //[ForeignKey(nameof(VehicleId))]
        //public virtual Vehicle Vehicle { get; set; }
    }
}
