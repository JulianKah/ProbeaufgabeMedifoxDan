using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProbeaufgabeMedifoxDan.Services
{
    public static class HashServiceSHA
    {
        public static string Hash(string toHash)
        {
            using(SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(toHash));
                string hash = Convert.ToBase64String(hashBytes);
                return hash;
            }
        }
    }
}
