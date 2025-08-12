// Dosya: Models/SapUserModel.cs

namespace StajProject.Models
{
    public class SAPUserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }  // Ekranda göstermeyeceksen silebilirsin
        public string Role { get; set; }
        public string IlKodu { get; set; }  // Yeni eklenen il_kodu field'ı
    }
}
