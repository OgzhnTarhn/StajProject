namespace StajProject.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        // Gerekirse rol veya ekstra alanlar
        public string Role { get; set; }
        public string IlKodu { get; set; }  // Yeni eklenen il_kodu field'ı
    }
}
