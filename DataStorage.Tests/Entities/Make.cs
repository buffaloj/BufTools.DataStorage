using BufTools.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataInjection.EFCore.Tests.Entities
{
    [Entity]
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
