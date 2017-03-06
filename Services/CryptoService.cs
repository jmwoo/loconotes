using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace loconotes.Services
{
    public interface ICryptoService
    {
        string Hash(string value);
    }

    public class CryptoService : ICryptoService
    {
        public string HashingAlgorithm => "SHA2_512";
        public string Hash(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("value is null/empty");

            var hashedBytes = GetHashedBytes(value);
            var hashedString = ConvertBytesToString(hashedBytes);
            return hashedString;
        }

        private byte[] GetHashedBytes(string value)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        private string ConvertBytesToString(byte[] value)
        {
            return Convert.ToBase64String(value);
        }
    }
}
