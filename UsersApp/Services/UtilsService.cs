namespace UsersApp.Services
{
    using System.Security.Cryptography;
    using System.Text;
    public static class UtilsService
    {
        public static string ConvertToSHA256(string? text)
        {
            string hash = string.Empty;

            using (SHA256 sha256= SHA256.Create())
            {
                // get hash from the text received
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(text??""));

                // convert array byte to string
                foreach (byte b in hashValue)
                {
                    hash += $"{b:X2}";
                }
            }

            return hash;
        }

        public static string GenerateToken()
        {
            string token = Guid.NewGuid().ToString("N");
            return token;
        }

        public static object GetDBValue(object? value)
        {
            return value == null ? DBNull.Value : value;
        }
    }
}
