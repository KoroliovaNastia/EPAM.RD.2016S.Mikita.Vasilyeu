using DAL.Interface;
using Storage;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly UserStorage storage;

        public UserRepository()
        {
            storage = new UserStorage();
        }

        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            if (!User.validator.Validate(user))
                throw new ArgumentException();
            storage.Users.Add(user);
            return user.Id;
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            User userToDelete = storage.Users.SingleOrDefault(u => u.Id == user.Id);
            if (userToDelete != null)
                storage.Users.Remove(userToDelete);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return storage.Users.ToList();
        }

        public int[] SearchForUsers(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException();
            List<User> foundUsers = storage.Users.Where(criteria).ToList();
            int[] ids = null;
            if (foundUsers != null && foundUsers.Count != 0)
            {
                ids = new int[foundUsers.Count];
                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i] = foundUsers[i].Id;
                }
            }
            return ids;
        }
    }
}
