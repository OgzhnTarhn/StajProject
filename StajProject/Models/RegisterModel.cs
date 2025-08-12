using System.ComponentModel.DataAnnotations;
using StajProject.Attributes;
namespace StajProject.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [StrongPassword] // Yeterli! Mesajları attribute yönetiyor.
        public string Password { get; set; }

        public string Role { get; set; }
        public string IlKodu { get; set; }  // Yeni eklenen il_kodu field'ı
        public string Message { get; set; }
    }


}
