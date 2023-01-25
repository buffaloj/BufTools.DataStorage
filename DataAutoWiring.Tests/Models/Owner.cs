using BufTools.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataInjection.EFCore.Tests.Models
{
    [Entity]
    public class Owner
    {
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }
    }
}
