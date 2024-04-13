using UsersApp.Models.Role;

namespace UsersApp.Models.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmedPassword { get; set; }
        public bool RestorePassword { get; set; }
        public bool ConfirmPassword { get; set; }
        public string? Token { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLoggedInAt { get; set; }
        public bool IsVerified { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswer { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public RoleDTO Role { get; set; } = new RoleDTO();
    }
}
