using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models.Email
{
    public class EmailDTO
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public string? About { get; set; }
        public string? Content { get; set; }
    }
}
