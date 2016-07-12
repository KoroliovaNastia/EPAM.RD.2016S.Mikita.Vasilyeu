using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    public class UserRepository : IUserRepository
    {
        private List<User> users;

        public UserRepository()
        {
            users = new List<User>();
        }

        public int Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            if (!User.validator.Validate(user))
                throw new ArgumentException();
            users.Add(user);
            return user.Id;
        }

        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            User userToDelete = users.SingleOrDefault(u => u.Id == user.Id);
            if (userToDelete != null)
                users.Remove(userToDelete);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return users.ToList();
        }

        public int[] SearchForUser(Func<User, bool> criteria)
        {
            List<User> foundUsers = users.Where(criteria).ToList();
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
