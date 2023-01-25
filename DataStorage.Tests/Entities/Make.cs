using BufTools.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataInjection.EFCore.Tests.Entities
{
    [Entity("make")]
    public class Make
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("manufacturer")]
        public string MakeName { get; set; }
    }
}
