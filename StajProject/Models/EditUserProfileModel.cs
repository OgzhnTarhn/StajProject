// Models/EditUserProfileModel.cs
namespace StajProject.Models
{
    public class EditUserProfileModel
    {
        // Ortak alanlar
        public string Username { get; set; } // Eski kullanıcı adı
        public string OldPassword { get; set; } // Kullanıcı şifresini değiştirmek isterse zorunlu

        // Kullanıcı değiştirebilir
        public string NewUsername { get; set; }
        public string NewPassword { get; set; }

        // Admin değiştirebilir
        public string Role { get; set; }

        public string Message { get; set; }
    }
}
