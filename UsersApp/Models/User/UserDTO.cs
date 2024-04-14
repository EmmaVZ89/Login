using System.Data.SqlClient;
using UsersApp.DataDB.Role;
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

        public void MapFrom(SqlDataReader dr)
        {
            FirstName = dr["FirstName"].ToString();
            LastName = dr["LastName"].ToString();
            if (dr["DateOfBirth"] != DBNull.Value)
            {
                DateOfBirth = (DateTime)dr["DateOfBirth"];
            }
            PhoneNumber = dr["PhoneNumber"].ToString();
            Address = dr["Address"].ToString();
            Email = dr["Email"].ToString();
            Password = dr["Password"].ToString();
            ConfirmedPassword = dr["ConfirmedPassword"].ToString();
            RestorePassword = (bool)dr["RestorePassword"];
            ConfirmPassword = (bool)dr["ConfirmPassword"];
            Token = dr["Token"].ToString();
            ProfilePicture = dr["ProfilePicture"].ToString();
            IsActive = (bool)dr["IsActive"];
            if (dr["CreatedAt"] != DBNull.Value)
            {
                CreatedAt = (DateTime)dr["CreatedAt"];
            }
            if (dr["LastLoggedInAt"] != DBNull.Value)
            {
                LastLoggedInAt = (DateTime)dr["LastLoggedInAt"];
            }
            IsVerified = (bool)dr["IsVerified"];
            SecurityQuestion = dr["SecurityQuestion"].ToString();
            SecurityAnswer = dr["SecurityAnswer"].ToString();
            TwoFactorEnabled = (bool)dr["TwoFactorEnabled"];

            var RoleId = Convert.ToInt32(dr["RoleId"].ToString());
            if (RoleId > 0)
            {
                Role = DBRole.Get(RoleId);
            }
        }

    }
}
