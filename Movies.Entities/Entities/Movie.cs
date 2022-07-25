using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Entities.Entities
{
    public class Movie : EntityBase
    {
        [Required]
        public string Title { get; set; }
        public string Poster { get; set; }
        [Required]
        public string Director { get; set; }
        public DateTime Released { get; set; }
        public bool IsPrivate { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
