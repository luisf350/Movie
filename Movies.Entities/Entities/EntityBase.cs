using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Entities.Entities
{
    public class EntityBase
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTimeOffset CreationDate { get; set; }

        public DateTimeOffset ModificationDate { get; set; }
    }
}
