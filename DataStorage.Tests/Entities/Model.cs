﻿using BufTools.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataInjection.EFCore.Tests.Entities
{
    [Entity]
    [Table("model")]
    public class Model
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("make_id")]
        public int MakeId { get; set; }

        [Column("name")]
        public string ModelName { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(MakeId))]
        public virtual Make Make { get; set; }
    }
}
