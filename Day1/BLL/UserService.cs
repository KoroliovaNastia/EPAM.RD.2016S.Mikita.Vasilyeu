using BLL.Interface;
using BLL.Models;
using DAL;
using DAL.Interface;
using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BLL.Mappers;
using BLL.Modes;

namespace BLL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository storage;
        private readonly IMode mode;

        public UserService(IUserRepository storage, IMode mode)
        {
            if(mode.IsActivated)
                throw new ArgumentException();
            mode.Activate();
            this.mode = mode;
            this.storage = storage;
        }

        public int Add(BllUser user)
        {
            mode.Add();
            return storage.Add(user.ToDalUser());
        }

        public void Delete(BllUser user)
        {
            mode.Delete();
            storage.Delete(user.ToDalUser());
        }

        public IEnumerable<BllUser> GetAllUsers()
        {
            return storage.GetAll().Select(user=>user.ToBllUser());
        }

        public int[] SearchForUsers(Func<BllUser, bool> criteria)
        {
            Func<DalUser, bool> predicate = user => criteria.Invoke(user.ToBllUser());
            return storage.GetByPredicate(predicate);
        }

        public void Save()
        {
            storage.Save();
        }

        public void Load()
        {
            storage.Load();
        }
    }
}
