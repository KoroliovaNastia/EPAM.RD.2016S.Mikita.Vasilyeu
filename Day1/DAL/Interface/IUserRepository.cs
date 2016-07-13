using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface IUserRepository
    {
        int Add(User user);
        int[] SearchForUsers(Func<User, bool> criteria);
        void Delete(User user);
        IEnumerable<User> GetAllUsers();
    }
}
