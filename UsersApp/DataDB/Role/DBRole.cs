using UsersApp.Models;
using System.Data;
using System.Data.SqlClient;
using UsersApp.Models.User;
using UsersApp.Services;
using UsersApp.Models.Role;

namespace UsersApp.DataDB.Role
{
    public class DBRole
    {
        private static string _SQLString = "Server=.;DataBase=DBPRUEBA; Trusted_Connection=True; TrustServerCertificate=True;";

        public static RoleDTO Get(int idRole)
        {
            RoleDTO role = null;

            try
            {
                using (SqlConnection oConnection = new SqlConnection(_SQLString))
                {
                    string query = @"SELECT * FROM ROLES";
                    query += @" WHERE Id=@Id";

                    SqlCommand cmd = new SqlCommand(query, oConnection);
                    cmd.Parameters.AddWithValue("@Id", idRole);

                    cmd.CommandType = CommandType.Text;

                    oConnection.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            role = new RoleDTO();
                            role.MapFrom(dr);
                        }
                    }
                }

                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
