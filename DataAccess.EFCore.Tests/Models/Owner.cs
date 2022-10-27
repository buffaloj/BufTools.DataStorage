using DataAccess.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.EFCore.Tests.Models
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
