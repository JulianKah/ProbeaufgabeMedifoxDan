using ProbeaufgabeMedifoxDan.Interfaces;
using ProbeaufgabeMedifoxDan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeaufgabeMedifoxDan.Services.Mock
{
    public class MockUserService : IUserService
    {
        private List<User> userStorage;
        public MockUserService()
        {
            this.userStorage = new List<User>();
        }
        public User Create(User userToSave)
        {
            if (this.userStorage.Count > 0)
            {
                int maxKey = Int16.Parse(this.userStorage.Max(u => u.UserId));
                userToSave.UserId = (maxKey + 1).ToString();
            }
            else
            {
                userToSave.UserId = "0";
            }

            if (this.userStorage.Where(u => u.Email == userToSave.Email).FirstOrDefault() == null)
            {
                if (userToSave.Nickname == "Not set" || this.userStorage.Where(u => u.Nickname == userToSave.Nickname).FirstOrDefault() == null)
                {
                    this.userStorage.Add(userToSave);
                    return userToSave;
                }
            }
            throw new InvalidDataException("User already exists!");
        }

        public User GetByMail(string userMail)
        {
            return this.userStorage.Where(u => u.Email == userMail).FirstOrDefault();
        }

        public User GetById(string userId)
        {
            return this.userStorage.Where(u => u.UserId == userId).FirstOrDefault();
        }

        public User GetByNickname(string nickname)
        {
            if (nickname != "Not set")
            {
                return this.userStorage.Where(u => u.Nickname == nickname).FirstOrDefault();
            }
            throw new InvalidDataException("Nickname cant be Not set!");
        }

        public IEnumerable<User> GetAll()
        {
            return this.userStorage;
        }

        public User Update(User user)
        {
            int targetIndext = this.userStorage.FindIndex(u => u.UserId == user.UserId);
            if (targetIndext != -1)
            {
                this.userStorage[targetIndext] = user;
            }
            return user;
        }

        public bool DeleteById(string userId)
        {
            User toRemove = GetById(userId);
            return this.userStorage.Remove(toRemove);
        }
    }
}
