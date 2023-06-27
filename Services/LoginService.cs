using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;

namespace ProbeaufgabeMedifoxDan.Services
{
    public class LoginService
    {
        IUserService userService;
        List<string> activeLoginTokens;
        public LoginService(IUserService userService) {
            this.userService = userService;
            this.activeLoginTokens = new List<string>();
        }

        public bool CheckLoginTokenValidity(string token)
        {
            CheckLoginTokens();
            return activeLoginTokens.Contains(token);
        }

        public string Login(UserLogin userLogin)
        {
            string pwHash = HashServiceSHA.Hash(userLogin.Password);
            User storedUser = null;

            if(String.IsNullOrEmpty(userLogin.Email))
            {
                storedUser = userService.GetByNickname(userLogin.Nickname);
            }
            else
            {
                storedUser = userService.GetByMail(userLogin.Email);
            }
            
            if(storedUser == null)
            {
                return null;
            }

            if(pwHash == storedUser.Password)
            {
                string jwt = GenerateToken(storedUser);
                activeLoginTokens.Add(jwt);
                return jwt;
            }
            else
            {
                return null;
            }
        }

        public void RequestPasswordReset(string email)
        {
            MailService mailService = MailService.GetInstance();
            mailService.SendRequestResetPasswordMail(email);

            User storedUser = this.userService.GetByMail(email);
            if(storedUser == null)
            {
                return;
            }

            storedUser.Password = HashServiceSHA.Hash(PasswordGenerator.GenerateRandomPassword());
            userService.Update(storedUser); 
        }

        private void CheckLoginTokens()
        {
            List<string> tokensToRemove = new List<string>();
            this.activeLoginTokens.ForEach(token =>
            {
                if (!JWTService.IsValid(token))
                {
                    tokensToRemove.Add(token);
                }
            });
            tokensToRemove.ForEach(token =>
            {
                activeLoginTokens.Remove(token);
            });
        }

        private string GenerateToken(User storedUser)
        {
            return JWTService.CreateToken(60, storedUser);
        }
    }
}
