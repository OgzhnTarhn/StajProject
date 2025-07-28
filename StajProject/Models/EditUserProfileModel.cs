// Models/EditUserProfileModel.cs
namespace StajProject.Models
{
    public class EditUserProfileModel
    {
        public string Username { get; set; }
        public string NewUsername { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Message { get; set; }
    }
}
