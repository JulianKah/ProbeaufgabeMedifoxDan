using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeaufgabeMedifoxDan.Services
{
    public class MailService
    {
        private List<string> unapprovedUserRegistrationMails = new List<string>();
        private MailService() { }

        private static MailService instance;

        public static MailService GetInstance()
        {
            if (instance == null)
            {
                instance = new MailService();
            }
            return instance;
        }

        public void SendRegistrationMail(string userMail)
        {
            unapprovedUserRegistrationMails.Add(userMail);
        }

        public void ApproveRegistrationMail(string userMail, IUserService userService)
        {
            if(unapprovedUserRegistrationMails.Contains(userMail))
            {
                unapprovedUserRegistrationMails.Remove(userMail);
                ApproveUser(userMail, userService);
            }
        }

        private void ApproveUser(string userMail, IUserService userService)
        {
            User user = userService.GetByMail(userMail);
            user.Approved = true;

            userService.Update(user);
        }

        public void SendRequestResetPasswordMail(string userMail)
        {
            Console.WriteLine("Sending PasswordResetMail to: " + userMail);
            Console.WriteLine("Link Clicked!");          
        }

    }
}
