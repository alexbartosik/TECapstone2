using System.ComponentModel.DataAnnotations;

namespace TenmoClient.Data
{
    /// <summary>
    /// Model to provide login parameters
    /// </summary>
    public class LoginUser
    {
        [StringLength(50, MinimumLength = 1)]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
