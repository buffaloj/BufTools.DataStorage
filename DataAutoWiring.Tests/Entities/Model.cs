using BufTools.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataInjection.EFCore.Tests.Entities
{
    [Entity("model")]
    public class Model
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("make_id")]
        public int MakeId { get; set; }

        [Column("name")]
        public string ModelName { get; set; }

        [JsonIgnore]
        [ForeignKey("MakeId")]
        public virtual Make Make { get; set; }
    }
}
