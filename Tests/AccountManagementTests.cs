using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;
using ProbeaufgabeMedifoxDan.Services;
using ProbeaufgabeMedifoxDan.Services.Mock;

namespace ProbeaufgabeMedifoxDanTests.Tests
{
    [TestClass]
    public class AccountManagementTests
    {
        [TestMethod]
        public void TestGetCurrentUser()
        {
            MockUserService userService = new MockUserService();
            LoginService loginService = new LoginService(userService);
            AccountManagementService accountManagementService = new AccountManagementService(userService);
            User storedUser = RegisterTestUser(userService);

            UserLogin userLogin = new UserLogin();
            userLogin.Email = "test@mail.com";
            userLogin.Password = "pw123#";

            string token = loginService.Login(userLogin);


            User currentUser = accountManagementService.GetCurrentUser(token);
            Assert.IsNotNull(currentUser);
        }

        [TestMethod]
        public void TestChangePassword()
        {
            MockUserService userService = new MockUserService();
            AccountManagementService accountManagementService = new AccountManagementService(userService);
            User storedUser = RegisterTestUser(userService);
            string storedUserPasswordHash = storedUser.Password;

            accountManagementService.ChangePassword(storedUser.UserId, "123");
            User updatedUser = userService.GetById(storedUser.UserId);
            Assert.AreNotEqual(storedUserPasswordHash, updatedUser.Password);
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            MockUserService userService = new MockUserService();
            AccountManagementService accountManagementService = new AccountManagementService(userService);
            User storedUser = RegisterTestUser(userService);

            accountManagementService.DeleteUser(storedUser.UserId);
            Assert.IsNull(userService.GetById(storedUser.UserId));
        }

        private User RegisterTestUser(IUserService userService)
        {
            RegistrationService registrationService = new RegistrationService(userService);
            UserRegistration newUser = new UserRegistration();
            newUser.Email = "test@mail.com";
            newUser.Nickname = "TestUser";
            newUser.Password = "pw123#";

            return registrationService.RegisterUser(newUser);
        }
    }
}
