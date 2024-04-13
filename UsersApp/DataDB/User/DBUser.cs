namespace UsersApp.DataDB.User
{
    using UsersApp.Models;
    using System.Data;
    using System.Data.SqlClient;
    using UsersApp.Models.User;

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
                    string query = @"INSERT INTO USERS(
                                                    Id,
                                                    FirstName,
                                                    LastName,
                                                    DateOfBirth,
                                                    PhoneNumber,
                                                    Address,
                                                    Email,
                                                    Password,
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
                                                    RoleId";
                    query += @" VALUES(@FirstName,
                                        @LastName,
                                        @DateOfBirth,
                                        @PhoneNumber,
                                        @Address,
                                        @Email,
                                        @Password,
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
                                        @RoleId)";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);
                    cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@RestorePassword", user.RestorePassword);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", user.ConfirmPassword);
                    cmd.Parameters.AddWithValue("@Token", user.Token);
                    cmd.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedAt", user.CreatedAt);
                    cmd.Parameters.AddWithValue("@LastLoggedInAt", user.LastLoggedInAt);
                    cmd.Parameters.AddWithValue("@IsVerified", user.IsVerified);
                    cmd.Parameters.AddWithValue("@SecurityQuestion", user.SecurityQuestion);
                    cmd.Parameters.AddWithValue("@SecurityAnswer", user.SecurityAnswer);
                    cmd.Parameters.AddWithValue("@TwoFactorEnabled", user.TwoFactorEnabled);
                    cmd.Parameters.AddWithValue("@RoleId", user.Role.Id);

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
                    string query = @"SELECT FirstName,RestorePassword,ConfirmPassword FROM USERS";
                    query += @" WHERE Email=@Email AND Password=@Password";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new UserDTO()
                            {
                                FirstName = dr["FirstName"].ToString(),
                                RestorePassword = (bool)dr["RestorePassword"],
                                ConfirmPassword = (bool)dr["ConfirmPassword"]
                            };
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
                    string query = @"SELECT FirstName,Password,RestorePassword,ConfirmPassword,Token FROM USERS";
                    query += @" WHERE Email=@Email";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Email", email);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    using(SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            user = new UserDTO()
                            {
                                FirstName = dr["FirstName"].ToString(),
                                Password = dr["Password"].ToString(),
                                RestorePassword = (bool)dr["RestorePassword"],
                                ConfirmPassword = (bool)dr["ConfirmPassword"],
                                Token = dr["Token"].ToString()
                            };
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
                                     Password=@Password 
                                     WHERE Token=@Token";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@RestorePassword", restore);
                    cmd.Parameters.AddWithValue("@Password", password);
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
