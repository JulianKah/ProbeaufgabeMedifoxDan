using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;
using ProbeaufgabeMedifoxDan.Services;
using ProbeaufgabeMedifoxDan.Services.Mock;

namespace ProbeaufgabeMedifoxDanTests.Tests
{
    [TestClass]
    public class LoginTests
    {
        private object loginService;

        [TestMethod]
        public void TestValidEmailLogin()
        {
            MockUserService userService = new MockUserService();
            User storedUser = RegisterTestUser(userService);

            UserLogin userLogin = new UserLogin();
            userLogin.Email = "test@mail.com";
            userLogin.Password = "pw123#";


            LoginService loginService = new LoginService(userService);
            string loginResult = loginService.Login(userLogin);

            Assert.IsNotNull(loginResult);
        }

        [TestMethod]
        public void TestValidNicknameLogin()
        {
            MockUserService userService = new MockUserService();
            User storedUser = RegisterTestUser(userService);

            UserLogin userLogin = new UserLogin();
            userLogin.Nickname = "TestUser";
            userLogin.Password = "pw123#";


            LoginService loginService = new LoginService(userService);
            string loginResult = loginService.Login(userLogin);

            Assert.IsNotNull(loginResult);
        }
         
        [TestMethod]
        public void TestInvalidLogin()
        {
            MockUserService userService = new MockUserService();
            UserLogin userLogin = new UserLogin();
            userLogin.Email = "test@mail.com";
            userLogin.Password = "pw123#";


            LoginService loginService = new LoginService(userService);
            string loginResult = loginService.Login(userLogin);

            Assert.IsNull(loginResult);
        }

        [TestMethod]
        public void TestValidLoginToken()
        {
            MockUserService userService = new MockUserService();
            User storedUser = RegisterTestUser(userService);

            UserLogin userLogin = new UserLogin();
            userLogin.Email = "test@mail.com";
            userLogin.Password = "pw123#";

            LoginService loginService = new LoginService(userService);
            string loginResult = loginService.Login(userLogin);
            bool isLoginTokenValid = loginService.CheckLoginTokenValidity(loginResult);

            Assert.IsTrue(isLoginTokenValid);
        }

        [TestMethod]
        public void TestInvalidLoginToken()
        {
            MockUserService userService = new MockUserService();
            LoginService loginService = new LoginService(userService);
            bool isLoginTokenValid = loginService.CheckLoginTokenValidity("123dfjl");

            Assert.IsFalse(isLoginTokenValid);
        }

        [TestMethod]
        public void TestPasswordReset()
        {
            MockUserService userService = new MockUserService();
            User storedUser = RegisterTestUser(userService);
            string storedUserPassword = storedUser.Password;
            LoginService loginService = new LoginService(userService);
            loginService.RequestPasswordReset("test@mail.com");

            User userAfterPasswordReset = userService.GetById(storedUser.UserId);
            Assert.AreNotEqual(userAfterPasswordReset.Password, storedUserPassword);
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
