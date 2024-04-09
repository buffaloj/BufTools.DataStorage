using BufTools.DataStore.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataStorage.Tests.Entities
{
    [StoredData]
    [Table("person")]
    public class Person
    {
        [Key]
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
