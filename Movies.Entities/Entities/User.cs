using System;
using Microsoft.EntityFrameworkCore;

namespace Movies.Entities.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class User : EntityBase
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FullName { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
