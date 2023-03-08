using System.ComponentModel.DataAnnotations;

namespace TenmoClient.Models
{
    public class User
    {
        [Required]
        public int UserId { get; set; }
        [Required, StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }
    }
}
