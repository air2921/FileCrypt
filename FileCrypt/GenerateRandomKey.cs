﻿using System.Security.Cryptography;

namespace FileCrypt
{
    internal class GenerateRandomKey
    {
        public byte[] GenerateKey()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] key = new byte[256];
                rng.GetBytes(key);
                return key;
            }
        }
    }
}
