using System.Security.Cryptography;

namespace FileCrypt
{
    internal class GenerateRandomKeySalt
    {
        public byte[] GenerateRandomKey()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] key = new byte[256];
                rng.GetBytes(key);
                return key;
            }
        }

        public byte[] GenerateRandomSalt()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[128];
                rng.GetBytes(salt);
                return salt;
            }
        }
    }
}