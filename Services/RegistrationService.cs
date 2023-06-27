using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;

namespace ProbeaufgabeMedifoxDan.Services
{
    public class RegistrationService
    {
        private IUserService userService;
        private MailService mailService;

        public RegistrationService(IUserService userService)
        {
            this.userService = userService;
            this.mailService = MailService.GetInstance();
        }

        public User RegisterUser(UserRegistration registration)
        {
            if (String.IsNullOrEmpty(registration.Email))
            {
                throw new InvalidDataException("Email is empty");
            }
            User created = null;
            if (registration.Password == null)
            {
                registration.Password = PasswordGenerator.GenerateRandomPassword();
            }
            string pwHash = HashServiceSHA.Hash(registration.Password);
            User newUser = new User();

            newUser.Email = registration.Email;
            newUser.Nickname = String.IsNullOrEmpty(registration.Nickname) ? "Not set" : registration.Nickname;
            newUser.Password = pwHash;
            newUser.RegistrationDate = DateTime.Now;
            newUser.LastUpdate = DateTime.Now;

            try
            {
                created = this.userService.Create(newUser);
            } 
            catch(InvalidDataException ex)
            {
                Console.WriteLine(ex.Message);
            }

            this.mailService.SendRegistrationMail(newUser.Email);
            return created;
        }

        public void ApproveUser(string userMail)
        {
            User toApprove = this.userService.GetByMail(userMail);
            toApprove.Approved = true;

            this.userService.Update(toApprove);
        }
    }
}
