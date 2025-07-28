namespace StajProject.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }      // Sadece admin panelinde kullanılacak!
        public string Message { get; set; }   // Sonuç mesajı (SAP'den gelen)
    }
}
