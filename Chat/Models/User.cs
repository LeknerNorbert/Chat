using System.ComponentModel.DataAnnotations;

namespace Chat.Models
{
    public class User
    {
        [Key]
        public string? Username { get; set; }
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string? Firstname { get; set; }
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string? Lastname { get; set; }
        public string? Middlename { get; set; }
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string? Email { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
    }
}
