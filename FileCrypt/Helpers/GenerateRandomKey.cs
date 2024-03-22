using System.Security.Cryptography;

namespace FileCrypt.Helpers
{
    internal class Generate : IGenerate
    {
        public byte[] GenerateKey()
        {
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            byte[] key = new byte[32];
            rng.GetBytes(key);
            return key;
        }
    }

    internal interface IGenerate
    {
        public byte[] GenerateKey();
    }
}
