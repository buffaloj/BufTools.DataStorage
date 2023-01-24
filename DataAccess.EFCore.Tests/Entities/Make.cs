using DataAccess.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EFCore.Tests.Entities
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
