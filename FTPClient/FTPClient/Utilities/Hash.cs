using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FTPClient.Utilities
{
    public class Hash
    {
        private HashAlgorithm algorithm;
        private string salt;
        private string stringToHash;
        public Hash(HashAlgorithm algorithm, string salt, string stringToHash)
        {
            this.algorithm = algorithm;
            this.salt = salt;
            this.stringToHash = stringToHash;
        }

        public string String()
        {
            byte[] textWithSaltBytes = Encoding.UTF8.GetBytes(string.Concat(stringToHash, salt));
            byte[] hashedBytes = algorithm.ComputeHash(textWithSaltBytes);

            return Convert.ToBase64String(hashedBytes);
        }
    }
}