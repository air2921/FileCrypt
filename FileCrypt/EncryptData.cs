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

        private static byte[] EncryptImage(byte[] fileData, byte[] key, byte[] salt)
        {
            using (Aes aes = Aes.Create())
            {
                using (Rfc2898DeriveBytes rfc2898 = new Rfc2898DeriveBytes(key, salt, 10000))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                aes.GenerateIV();

                using (MemoryStream memoryStream = new MemoryStream(fileData))
                {
                    using (MemoryStream encryptedStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(encryptedStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            memoryStream.CopyTo(cryptoStream);
                            cryptoStream.FlushFinalBlock();
                        }

                        byte[] encryptedData = encryptedStream.ToArray();
                        return encryptedData;
                    }
                }
            }
        }

        public void EncryptImageFile(string filePath, byte[] key, byte[] salt)
        {
            byte[] imageData;
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fs.CopyTo(memoryStream);
                    imageData = memoryStream.ToArray();
                }
            }

            byte[] encryptedData = EncryptImage(imageData, key, salt);

            using (FileStream encryptedFileStream = new FileStream(filePath, FileMode.Create))
            {
                encryptedFileStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        public void EncryptTxtFile(string filePath, byte[] key, byte[] salt)
        {
            string plainText = File.ReadAllText(filePath, Encoding.UTF8);
            byte[] encryptedData = EncryptText(Encoding.UTF8.GetBytes(plainText), key, salt);
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