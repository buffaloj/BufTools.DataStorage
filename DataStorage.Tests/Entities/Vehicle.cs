using BufTools.DataStore.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataStorage.Tests.Entities
{
    [StoredData]
    [Table("vehicle")]
    public class Vehicle
    {
        [Key]
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
