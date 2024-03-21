using System.Security.Cryptography;

namespace FileCrypt.Cryptography
{
    internal interface ICypher
    {
        public Task<bool> CypherFileAsync(string filePath, byte[] key, CancellationToken cancellationToken = default);
    }

    public class Encrypt : ICypher
    {
        private async Task EncryptionAsync(Stream src, Stream target, byte[] key, CancellationToken cancellationToken = default)
        {
            try
            {
                using var aes = Aes.Create();

                byte[] iv = aes.IV;
                await target.WriteAsync(iv, cancellationToken);
                using (Rfc2898DeriveBytes rfc2898 = new(key, iv, 1000, HashAlgorithmName.SHA256))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                using CryptoStream cryptoStream = new(target, aes.CreateEncryptor(), CryptoStreamMode.Write);
                await src.CopyToAsync(cryptoStream, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CypherFileAsync(string filePath, byte[] key, CancellationToken cancellationToken = default)
        {
            try
            {
                string tmp = $"{filePath}.tmp";
                using (var source = File.OpenRead(filePath))
                using (var target = File.Create(tmp))
                {
                    await EncryptionAsync(source, target, key, cancellationToken);
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
        private async Task DecryptionAsync(Stream source, Stream target, byte[] key, CancellationToken cancellationToken)
        {
            try
            {
                using var aes = Aes.Create();

                byte[] iv = new byte[aes.BlockSize / 8];
                await source.ReadAsync(iv, cancellationToken);
                aes.IV = iv;
                using (Rfc2898DeriveBytes rfc2898 = new(key, iv, 1000, HashAlgorithmName.SHA256))
                {
                    aes.Key = rfc2898.GetBytes(aes.KeySize / 8);
                }

                using CryptoStream cryptoStream = new(source, aes.CreateDecryptor(), CryptoStreamMode.Read);
                await cryptoStream.CopyToAsync(target, cancellationToken);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CypherFileAsync(string filePath, byte[] key, CancellationToken cancellationToken)
        {
            try
            {
                string tmp = $"{filePath}.tmp";
                using (var source = File.OpenRead(filePath))
                using (var target = File.Create(tmp))
                {
                    await DecryptionAsync(source, target, key, cancellationToken);
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
