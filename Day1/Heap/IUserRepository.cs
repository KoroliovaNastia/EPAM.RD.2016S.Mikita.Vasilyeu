using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heap
{
    public interface IUserRepository
    {
        int Add(User user);
        int[] SearchForUser(Func<User, bool> criteria);
        void Delete(User user);
        IEnumerable<User> GetAllUsers();
    }
}
