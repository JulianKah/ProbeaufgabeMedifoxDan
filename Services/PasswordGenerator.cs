using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeaufgabeMedifoxDan.Services
{
    public static class PasswordGenerator
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#+?";
        public static string GenerateRandomPassword()
        {
            Random random = new Random();
            var pw = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());

            return pw;
        }
    }
}
