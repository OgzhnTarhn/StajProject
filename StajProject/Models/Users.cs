using System.ComponentModel.DataAnnotations;

namespace StajProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(256)]
        public string PasswordHash { get; set; }

        [Required]
        public char Role { get; set; }   // 'S' (API User)
    }
}
