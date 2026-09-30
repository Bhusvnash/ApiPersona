using BCrypt;
namespace ApiPersonas.services
{
    public class Encoder
    {
        /// <summary>
        ///  generate salt and hash for password 
        /// </summary>
        /// <param name="password">  password to hash 
        /// </param>
        /// <returns>returns the hash password </returns>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        /// <summary>
        /// Verify password with hash
        /// </summary>
        /// <param name="password">password to verify
        /// </param>
        /// <param name="hash">hash password
        /// </param>
        /// <returns>true if password is correct false otherwise
        /// </returns>
        public static bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}