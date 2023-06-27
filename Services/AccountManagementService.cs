using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;

namespace ProbeaufgabeMedifoxDan.Services
{
    public class AccountManagementService
    {
        private IUserService _userService;
        public AccountManagementService(IUserService userService)
        {
            this._userService = userService;
        }
        public void ChangePassword(string userId, string newPassword)
        {
            User storedUser = this._userService.GetById(userId);
            if (storedUser != null)
            {
                string pwHash = HashServiceSHA.Hash(storedUser.Password);
                storedUser.Password = pwHash;
                this._userService.Update(storedUser);
            }
        }

        public void DeleteUser(string userId)
        {
            this._userService.DeleteById(userId);
        }

        public User GetCurrentUser(string token)
        {
            string userEmail = JWTService.GetEmailFromToken(token);
            if(userEmail != null)
            {
                User currentUser = this._userService.GetByMail(userEmail);
                return currentUser;
            }
            return null;
        }
    }
}
