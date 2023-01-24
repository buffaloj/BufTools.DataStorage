using DataAccess.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess.EFCore.Tests.Entities
{
    [Entity("vehicle")]
    public class Vehicle
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("model_id")]
        public int ModelId { get; set; }

        [Column("vin")]
        public string VIN { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ModelId))]
        public virtual Model Model { get; set; }

        public static Vehicle Example()
        {
            return new Vehicle
            {
                ModelId = 1,
                VIN = "11223344556677889"
            };
        }
    }
}
