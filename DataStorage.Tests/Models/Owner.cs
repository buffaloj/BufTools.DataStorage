using BufTools.DataStore.Schema;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStorage.Tests.Models
{
    [StoredData]
    public class Owner
    {
        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }
    }
}
