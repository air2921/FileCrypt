using System.Security.Cryptography;

namespace FileCrypt
{
    internal class EncryptData
    {
        private static byte[] Encrypt(byte[] data, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);

                    aes.GenerateIV();

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(aes.IV, 0, aes.IV.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] combinedData = new byte[salt.Length + aes.IV.Length + memoryStream.Length];
                            Buffer.BlockCopy(salt, 0, combinedData, 0, salt.Length);
                            Buffer.BlockCopy(aes.IV, 0, combinedData, salt.Length, aes.IV.Length);
                            Buffer.BlockCopy(memoryStream.GetBuffer(), 0, combinedData, salt.Length + aes.IV.Length, (int)memoryStream.Length);

                            return combinedData;
                        }
                    }
                }
            }
        }

        public void EncryptFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] data = File.ReadAllBytes(filePath);
            byte[] encryptedData = Encrypt(data, key, salt);
            File.WriteAllBytes(filePath, encryptedData);
        }
    }
}
