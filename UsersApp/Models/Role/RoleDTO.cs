using System.Data.SqlClient;

namespace UsersApp.Models.Role
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? DeactivationDate { get; set; }

        public void MapFrom(SqlDataReader dr)
        {
            Id = Convert.ToInt32(dr["Id"]);
            Name = dr["Name"].ToString();
            if (dr["DeactivationDate"] != DBNull.Value)
            {
                DeactivationDate = (DateTime)dr["DeactivationDate"];
            }
        }
    }
}
