using System.Security.Cryptography;
using System.Text;

namespace FileCrypt
{
    internal class DecryptData : IDecryptorTxtFile, IDecryptorImageFile
    {
        private static byte[] DecryptText(byte[] encryptedData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);

                    byte[] iv = new byte[aes.IV.Length];
                    Buffer.BlockCopy(encryptedData, salt.Length, iv, 0, iv.Length);
                    aes.IV = iv;

                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        byte[] encryptedText = new byte[encryptedData.Length - salt.Length - iv.Length];
                        Buffer.BlockCopy(encryptedData, salt.Length + iv.Length, encryptedText, 0, encryptedText.Length);

                        byte[] decryptedData = decryptor.TransformFinalBlock(encryptedText, 0, encryptedText.Length);
                        return decryptedData;
                    }
                }
            }
        }

        private static byte[] DecryptImage(byte[] encryptedData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                byte[] iv = new byte[aes.IV.Length];
                Buffer.BlockCopy(encryptedData, salt.Length, iv, 0, iv.Length);
                aes.IV = iv;

                using (MemoryStream encryptedStream = new MemoryStream(encryptedData, salt.Length + iv.Length, encryptedData.Length - salt.Length - iv.Length))
                using (MemoryStream decryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cryptoStream.CopyTo(decryptedStream);
                    }

                    byte[] decryptedData = decryptedStream.ToArray();
                    return decryptedData;
                }
            }
        }

        public void DecryptImageFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = DecryptImage(encryptedData, key, salt);

            using (MemoryStream decryptedStream = new MemoryStream(decryptedData))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    decryptedStream.CopyTo(fs);
                }
            }
        }

        public void DecryptTxtFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] encryptedData = File.ReadAllBytes(filePath);
            byte[] decryptedData = DecryptText(encryptedData, key, salt);
            string decryptedText = Encoding.UTF8.GetString(decryptedData);
            File.WriteAllText(filePath, decryptedText, Encoding.UTF8);
        }
    }

    public interface IDecryptorTxtFile
    {
        void DecryptTxtFile(string filePath, byte[] key, byte[] salt);
    }

    public interface IDecryptorImageFile
    {
        void DecryptImageFile(string filePath, byte[] key, byte[] salt);
    }
}