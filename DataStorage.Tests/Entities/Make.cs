using BufTools.DataStore.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorage.Tests.Entities
{
    [StoredData]
    [Table("make")]
    public class Make
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("manufacturer")]
        public string MakeName { get; set; }
    }
}
