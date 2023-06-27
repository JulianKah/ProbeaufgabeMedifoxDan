using ProbeaufgabeMedifoxDan.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeaufgabeMedifoxDan.Interfaces
{
    public interface IUserService
    {
        public User Create(User userToSave);
        public User GetByMail(string userMail);
        public User GetById(string userId);
        public User GetByNickname(string nickname);
        public User Update(User user);
        public bool DeleteById(string userId);
        public IEnumerable<User> GetAll();
    }
}
