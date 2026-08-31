using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string FirstName { get; set; } = null!;
        [Required]
        [StringLength(250)]
        public string LastName { get; set; } = null!;
        [Required]
        [StringLength(150)]
        public string Username { get; set; } = null!;
        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public Cart Cart { get; set; } = null!;

    }
}
