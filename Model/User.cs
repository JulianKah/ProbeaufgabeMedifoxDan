using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeaufgabeMedifoxDan.Model
{
    public class User
    {
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Nickname { get; set; }
        public string? Password { get; set; }
        public bool Approved { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime LastUpdate { get; set; }

        public void ApproveRegistration(IUserService userService)
        {
            MailService.GetInstance().ApproveRegistrationMail(this.Email, userService);
        }
    }
}
