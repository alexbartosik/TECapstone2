using System.ComponentModel.DataAnnotations;

namespace TenmoServer.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required, StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }
        [Required, StringLength(200, MinimumLength = 1)]
        public string PasswordHash { get; set; }
        [Required, StringLength(200, MinimumLength = 1)]
        public string Salt { get; set; }
        //public string Email { get; set; }
    }
}
