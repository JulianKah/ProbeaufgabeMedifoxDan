using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;
using ProbeaufgabeMedifoxDan.Services;
using ProbeaufgabeMedifoxDan.Services.Mock;

namespace ProbeaufgabeMedifoxDanTests.Tests
{
    [TestClass]
    public class RegistrationTests
    {
        [TestMethod]
        public void TestRegisterUserWithPasswordAndNickname()
        {
            MockUserService userService = new MockUserService();

            User storedUser = RegisterTestUser(userService);
            string newUserHash = HashServiceSHA.Hash("pw123#");

            Assert.IsNotNull(storedUser);
            Assert.AreEqual("test@mail.com", storedUser.Email);
            Assert.AreEqual("TestUser", storedUser.Nickname);
            Assert.AreEqual(storedUser.Password, newUserHash);
        }

        public void TestRegisterUserWithPasswordAndWithoutNickname()
        {
            MockUserService userService = new MockUserService();
            RegistrationService registrationService = new RegistrationService(userService);

            UserRegistration newUser = new UserRegistration();
            newUser.Email = "test@mail1.com";
            newUser.Password = "pw123#";
            User storedUser = registrationService.RegisterUser(newUser);

            string newUserHash = HashServiceSHA.Hash("pw123#");

            Assert.IsNotNull(storedUser);
            Assert.AreEqual("test@mail.com", storedUser.Email);
            Assert.AreEqual("Not set", storedUser.Nickname);
            Assert.AreEqual(storedUser.Password, newUserHash);
        }

        [TestMethod]
        public void TestRegisterUserWithOutPassword()
        {
            MockUserService userService = new MockUserService();
            RegistrationService registrationService = new RegistrationService(userService);
            UserRegistration newUser = new UserRegistration();
            newUser.Email = "test@mail.com";
            newUser.Nickname = "TestUser";


            User storedUser = registrationService.RegisterUser(newUser);

            Assert.IsNotNull(storedUser);
            Assert.IsNotNull(storedUser.Password);
        }

        [TestMethod]
        public void TestRegisterUserWithOutEmail()
        {
            MockUserService userService = new MockUserService();
            RegistrationService registrationService = new RegistrationService(userService);
            UserRegistration newUser = new UserRegistration();
            newUser.Email = "";
            newUser.Nickname = "TestUser";
            newUser.Password = "pw123#";


            Assert.ThrowsException<InvalidDataException>(() => registrationService.RegisterUser(newUser));
        }

        [TestMethod]
        public void TestRegister2SameEmailUserWithPassword()
        {
            MockUserService userService = new MockUserService();
            RegistrationService registrationService = new RegistrationService(userService);

            UserRegistration newUser1 = new UserRegistration();
            newUser1.Email = "test@mail1.com";
            newUser1.Nickname = "TestUser1";
            newUser1.Password = "pw123#";

            UserRegistration newUser2 = new UserRegistration();
            newUser2.Email = "test@mail1.com";
            newUser2.Nickname = "TestUser2";
            newUser2.Password = "pw123#";

            User storedUser = registrationService.RegisterUser(newUser1);
            User secondStoredUser = registrationService.RegisterUser(newUser2);


            Assert.IsNull(secondStoredUser);
        }

        [TestMethod]
        public void TestRegister2SameNicknameUserWithPassword()
        {
            MockUserService userService = new MockUserService();
            RegistrationService registrationService = new RegistrationService(userService);

            UserRegistration newUser1 = new UserRegistration();
            newUser1.Email = "test@mail1.com";
            newUser1.Nickname = "TestUser";
            newUser1.Password = "pw123#";

            UserRegistration newUser2 = new UserRegistration();
            newUser2.Email = "test@mail2.com";
            newUser2.Nickname = "TestUser";
            newUser2.Password = "pw123#";

            User storedUser = registrationService.RegisterUser(newUser1);
            User secondStoredUser = registrationService.RegisterUser(newUser2);


            Assert.IsNull(secondStoredUser);
        }

        [TestMethod]
        public void TestRegister3UserWithPassword()
        {
            MockUserService userService = new MockUserService();
            RegistrationService registrationService = new RegistrationService(userService);
            UserRegistration newUser1 = new UserRegistration();
            newUser1.Email = "test@mail1.com";
            newUser1.Nickname = "TestUser1";
            newUser1.Password = "pw123#";

            UserRegistration newUser2 = new UserRegistration();
            newUser2.Email = "test@mail2.com";
            newUser2.Nickname = "TestUser2";
            newUser2.Password = "pw123#";

            UserRegistration newUser3 = new UserRegistration();
            newUser3.Email = "test@mail3.com";
            newUser3.Nickname = "TestUser3";
            newUser3.Password = "pw123#";

            User storedUser1 = registrationService.RegisterUser(newUser1);
            User storedUser2 = registrationService.RegisterUser(newUser2);
            User storedUser3 = registrationService.RegisterUser(newUser3);
            int count = userService.GetAll().Count();

            Assert.AreEqual(count, 3);
        }


        [TestMethod]
        public void TestRegistrationApproval()
        {
            MockUserService userService = new MockUserService();

            User storedUser = RegisterTestUser(userService);
            storedUser.ApproveRegistration(userService);

            User userAfterApproval = userService.GetById(storedUser.UserId);
            Assert.IsTrue(userAfterApproval.Approved);
        }

        [TestMethod]
        public void TestRegistrationNoApproval()
        {
            MockUserService userService = new MockUserService();

            User storedUser = RegisterTestUser(userService);


            Assert.IsFalse(storedUser.Approved);
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
