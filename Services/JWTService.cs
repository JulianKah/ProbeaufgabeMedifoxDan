using Microsoft.IdentityModel.Tokens;
using ProbeaufgabeMedifoxDan.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProbeaufgabeMedifoxDan.Services
{
    public static class JWTService
    {
        private const string secretKey = "SecretSecretSecretSecretSecretSecret";
        public static string CreateToken(int validityMinutes, User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Nickname),
                new Claim(ClaimTypes.Authentication, user.Password)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.ToLocalTime().AddMinutes(validityMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static bool IsValid(string toCheck)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.ReadJwtToken(toCheck);

            return token.ValidTo > DateTime.Now.ToUniversalTime();
        }

        public static string GetEmailFromToken(string tokenToReadFrom)
        {
            if (!string.IsNullOrEmpty(tokenToReadFrom))
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken token = tokenHandler.ReadJwtToken(tokenToReadFrom);

                Claim c = token.Claims.Where(c => c.Type == "email").FirstOrDefault();

                return c.Value;
            }
            return null;
        }
    }
}
