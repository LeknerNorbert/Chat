using System.ComponentModel.DataAnnotations;

namespace Chat.Models.DTOs
{
    public class UserDto
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
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string? Password { get; set; }
    }
}
