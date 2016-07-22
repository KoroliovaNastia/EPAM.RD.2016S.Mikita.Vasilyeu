using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interface
{
    public interface IUserService
    {
        int Add(BllUser user);
        int[] SearchForUsers(Func<BllUser, bool> criteria);
        void Delete(BllUser user);
        List<BllUser> GetAllUsers();
        void Save();
        void Load();
    }
}
