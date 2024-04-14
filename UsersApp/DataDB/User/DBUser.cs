using UsersApp.Models;
using System.Data;
using System.Data.SqlClient;
using UsersApp.Models.User;
using UsersApp.Services;
using Org.BouncyCastle.Utilities.Collections;

namespace UsersApp.DataDB.User
{
    public class DBUser
    {
        private static string SQLString = "Server=.;DataBase=DBPRUEBA; Trusted_Connection=True; TrustServerCertificate=True;";

        public static bool Register(UserDTO user)
        {
            bool result = false;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"INSERT INTO USERS (
                                    FirstName,
                                    LastName,
                                    DateOfBirth,
                                    PhoneNumber,
                                    Address,
                                    Email,
                                    Password,
                                    ConfirmedPassword,
                                    RestorePassword,
                                    ConfirmPassword,
                                    Token,
                                    ProfilePicture,
                                    IsActive,
                                    CreatedAt,
                                    LastLoggedInAt,
                                    IsVerified,
                                    SecurityQuestion,
                                    SecurityAnswer,
                                    TwoFactorEnabled,
                                    RoleId
                                )";
                    query += @" VALUES (
                        @FirstName,
                        @LastName,
                        @DateOfBirth,
                        @PhoneNumber,
                        @Address,
                        @Email,
                        @Password,
                        @ConfirmedPassword,
                        @RestorePassword,
                        @ConfirmPassword,
                        @Token,
                        @ProfilePicture,
                        @IsActive,
                        @CreatedAt,
                        @LastLoggedInAt,
                        @IsVerified,
                        @SecurityQuestion,
                        @SecurityAnswer,
                        @TwoFactorEnabled,
                        @RoleId
                    )";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@FirstName", UtilsService.GetDBValue(user.FirstName));
                    cmd.Parameters.AddWithValue("@LastName", UtilsService.GetDBValue(user.LastName));
                    cmd.Parameters.AddWithValue("@DateOfBirth", UtilsService.GetDBValue(user.DateOfBirth));
                    cmd.Parameters.AddWithValue("@PhoneNumber", UtilsService.GetDBValue(user.PhoneNumber));
                    cmd.Parameters.AddWithValue("@Address", UtilsService.GetDBValue(user.Address));
                    cmd.Parameters.AddWithValue("@Email", UtilsService.GetDBValue(user.Email));
                    cmd.Parameters.AddWithValue("@Password", UtilsService.GetDBValue(user.Password));
                    cmd.Parameters.AddWithValue("@ConfirmedPassword", UtilsService.GetDBValue(user.Password));
                    cmd.Parameters.AddWithValue("@RestorePassword", UtilsService.GetDBValue(user.RestorePassword));
                    cmd.Parameters.AddWithValue("@ConfirmPassword", UtilsService.GetDBValue(user.ConfirmPassword));
                    cmd.Parameters.AddWithValue("@Token", UtilsService.GetDBValue(user.Token));
                    cmd.Parameters.AddWithValue("@ProfilePicture", UtilsService.GetDBValue(user.ProfilePicture));
                    cmd.Parameters.AddWithValue("@IsActive", UtilsService.GetDBValue(user.IsActive));
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LastLoggedInAt", UtilsService.GetDBValue(user.LastLoggedInAt));
                    cmd.Parameters.AddWithValue("@IsVerified", UtilsService.GetDBValue(user.IsVerified));
                    cmd.Parameters.AddWithValue("@SecurityQuestion", UtilsService.GetDBValue(user.SecurityQuestion));
                    cmd.Parameters.AddWithValue("@SecurityAnswer", UtilsService.GetDBValue(user.SecurityAnswer));
                    cmd.Parameters.AddWithValue("@TwoFactorEnabled", UtilsService.GetDBValue(user.TwoFactorEnabled));
                    //cmd.Parameters.AddWithValue("@RoleId", UtilsService.GetDBValue(user.Role.Id));
                    cmd.Parameters.AddWithValue("@RoleId", 2);// User

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0) result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserDTO Validate(string email, string password)
        {
            UserDTO user = null;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"SELECT * FROM USERS";
                    query += @" WHERE Email=@Email AND Password=@Password";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new UserDTO();
                            user.MapFrom(dr);
                        }
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserDTO ValidateLoggedUser(string email, string token)
        {
            UserDTO user = null;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"SELECT * FROM USERS";
                    query += @" WHERE Email=@Email AND Token=@Token";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Token", token);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new UserDTO();
                            user.MapFrom(dr);
                        }
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UserDTO Get(string? email)
        {
            UserDTO user = null;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"SELECT * FROM USERS";
                    query += @" WHERE Email=@Email";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Email", email);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new UserDTO();
                            user.MapFrom(dr);
                        }
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool RestoreUpdate(int restore, string password, string token)
        {
            bool result = false;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"UPDATE USERS SET 
                                     RestorePassword=@RestorePassword,
                                     Password=@Password,
                                     ConfirmedPassword=@ConfirmedPassword
                                     WHERE Token=@Token";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@RestorePassword", restore);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@ConfirmedPassword", password);
                    cmd.Parameters.AddWithValue("@Token", token);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0) result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Update(UserDTO user)
        {
            bool result = false;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"UPDATE USERS SET 
                                     FirstName=@FirstName,
                                     LastName=@LastName,
                                     DateOfBirth=@DateOfBirth,
                                     Address=@Address,
                                     Email=@Email,
                                     Password=@Password,
                                     ConfirmedPassword=@ConfirmedPassword,
                                     RestorePassword=@RestorePassword,
                                     ConfirmPassword=@ConfirmPassword,
                                     Token=@Token,
                                     IsActive=@IsActive,
                                     CreatedAt=@CreatedAt,
                                     LastLoggedInAt=@LastLoggedInAt,
                                     IsVerified=@IsVerified,
                                     SecurityQuestion=@SecurityQuestion,
                                     SecurityAnswer=@SecurityAnswer,
                                     TwoFactorEnabled=@TwoFactorEnabled
                                     WHERE Email=@Email";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@FirstName", UtilsService.GetDBValue(user.FirstName));
                    cmd.Parameters.AddWithValue("@LastName", UtilsService.GetDBValue(user.LastName));
                    cmd.Parameters.AddWithValue("@DateOfBirth", UtilsService.GetDBValue(user.DateOfBirth));
                    cmd.Parameters.AddWithValue("@PhoneNumber", UtilsService.GetDBValue(user.PhoneNumber));
                    cmd.Parameters.AddWithValue("@Address", UtilsService.GetDBValue(user.Address));
                    cmd.Parameters.AddWithValue("@Email", UtilsService.GetDBValue(user.Email));
                    cmd.Parameters.AddWithValue("@Password", UtilsService.GetDBValue(user.Password));
                    cmd.Parameters.AddWithValue("@ConfirmedPassword", UtilsService.GetDBValue(user.Password));
                    cmd.Parameters.AddWithValue("@RestorePassword", UtilsService.GetDBValue(user.RestorePassword));
                    cmd.Parameters.AddWithValue("@ConfirmPassword", UtilsService.GetDBValue(user.ConfirmPassword));
                    cmd.Parameters.AddWithValue("@Token", UtilsService.GetDBValue(user.Token));
                    cmd.Parameters.AddWithValue("@ProfilePicture", UtilsService.GetDBValue(user.ProfilePicture));
                    cmd.Parameters.AddWithValue("@IsActive", UtilsService.GetDBValue(user.IsActive));
                    cmd.Parameters.AddWithValue("@CreatedAt", UtilsService.GetDBValue(user.CreatedAt));
                    cmd.Parameters.AddWithValue("@LastLoggedInAt", UtilsService.GetDBValue(user.LastLoggedInAt));
                    cmd.Parameters.AddWithValue("@IsVerified", UtilsService.GetDBValue(user.IsVerified));
                    cmd.Parameters.AddWithValue("@SecurityQuestion", UtilsService.GetDBValue(user.SecurityQuestion));
                    cmd.Parameters.AddWithValue("@SecurityAnswer", UtilsService.GetDBValue(user.SecurityAnswer));
                    cmd.Parameters.AddWithValue("@TwoFactorEnabled", UtilsService.GetDBValue(user.TwoFactorEnabled));

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0) result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Confirm(string token)
        {
            bool result = false;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(SQLString))
                {
                    string query = @"UPDATE USERS SET 
                                     ConfirmPassword=1 
                                     WHERE Token=@Token";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Token", token);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    int affectedRows = cmd.ExecuteNonQuery();

                    if (affectedRows > 0) result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
