using DataAccess.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess.EFCore.Tests.Entities
{
    [Entity("person")]
    public class Person
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [JsonIgnore]
        public ICollection<PersonCar> Cars { get; set; }
    }
}
