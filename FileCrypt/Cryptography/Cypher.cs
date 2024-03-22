using System.Security.Cryptography;

namespace FileCrypt.Cryptography
{
    internal interface ICypher
    {
        public Task<bool> CypherFileAsync(string filePath, byte[] key);
    }

    public class Encrypt : ICypher
    {
        private async Task EncryptionAsync(Stream src, Stream target, byte[] key)
        {
            try
            {
                using var aes = Aes.Create();

                byte[] iv = aes.IV;
                await target.WriteAsync(iv);
                using (Rfc2898DeriveBytes rfc2898 = new(key, iv, 1000, HashAlgorithmName.SHA256))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                using CryptoStream cryptoStream = new(target, aes.CreateEncryptor(), CryptoStreamMode.Write);
                await src.CopyToAsync(cryptoStream);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CypherFileAsync(string filePath, byte[] key)
        {
            try
            {
                string tmp = $"{filePath}.tmp";
                using (var source = File.OpenRead(filePath))
                using (var target = File.Create(tmp))
                {
                    await EncryptionAsync(source, target, key);
                }
                File.Move(tmp, filePath, true);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return false;
            }
        }
    }

    public class Decrypt : ICypher
    {
        private async Task DecryptionAsync(Stream source, Stream target, byte[] key)
        {
            try
            {
                using var aes = Aes.Create();

                byte[] iv = new byte[aes.BlockSize / 8];
                await source.ReadAsync(iv);
                aes.IV = iv;
                using (Rfc2898DeriveBytes rfc2898 = new(key, iv, 1000, HashAlgorithmName.SHA256))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                using CryptoStream cryptoStream = new(source, aes.CreateDecryptor(), CryptoStreamMode.Read);
                await cryptoStream.CopyToAsync(target);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CypherFileAsync(string filePath, byte[] key)
        {
            try
            {
                string tmp = $"{filePath}.tmp";
                using (var source = File.OpenRead(filePath))
                using (var target = File.Create(tmp))
                {
                    await DecryptionAsync(source, target, key);
                }
                File.Move(tmp, filePath, true);

                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return false;
            }
        }
    }
}
