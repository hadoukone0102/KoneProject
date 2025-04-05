using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KoneProject.Models
{
    public class UserModel: IdentityUser
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public byte[] PasswordSalt { get; set; } = new byte[0];
        [Required]
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Constructor
        public UserModel()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
