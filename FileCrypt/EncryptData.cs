using System.Security.Cryptography;
using System.Text;

namespace FileCrypt
{
    internal class EncryptData : IEncryptorTxtFile, IEncryptorImageFile
    {
        private static byte[] EncryptText(byte[] data, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);

                    aes.GenerateIV();
                    byte[] iv = aes.IV;

                    using (ICryptoTransform encryptor = aes.CreateEncryptor())
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        memoryStream.Write(iv, 0, iv.Length);

                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] encryptedData = memoryStream.ToArray();
                            byte[] combinedData = new byte[salt.Length + encryptedData.Length];
                            Buffer.BlockCopy(salt, 0, combinedData, 0, salt.Length);
                            Buffer.BlockCopy(encryptedData, 0, combinedData, salt.Length, encryptedData.Length);

                            return combinedData;
                        }
                    }
                }
            }
        }

        private static byte[] EncryptImage(byte[] fileData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                aes.GenerateIV();
                byte[] iv = aes.IV;

                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    encryptedStream.Write(salt, 0, salt.Length);
                    encryptedStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileData, 0, fileData.Length);
                        cryptoStream.FlushFinalBlock();
                    }

                    byte[] encryptedData = encryptedStream.ToArray();
                    return encryptedData;
                }
            }
        }

        public void EncryptImageFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] imageData = File.ReadAllBytes(filePath);
            byte[] encryptedData = EncryptImage(imageData, key, salt);

            using (FileStream encryptedFileStream = new FileStream(filePath, FileMode.Create))
            {
                encryptedFileStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        public void EncryptTxtFile(string filePath, byte[] key, byte[] salt)
        {
            string plainText = File.ReadAllText(filePath, Encoding.UTF8);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedData = EncryptText(plainTextBytes, key, salt);
            File.WriteAllBytes(filePath, encryptedData);
        }
    }

    public interface IEncryptorTxtFile
    {
        void EncryptTxtFile(string filePath, byte[] key, byte[] salt);
    }

    public interface IEncryptorImageFile
    {
        void EncryptImageFile(string filePath, byte[] key, byte[] salt);
    }
}