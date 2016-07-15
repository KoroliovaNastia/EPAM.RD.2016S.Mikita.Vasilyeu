using DAL.Interface;
using DAL.Models;
using Storage;
using Storage.Interface;
using Storage.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DAL.Mappers;
using DAL.Modes;

namespace DAL
{
    public class UserRepository : IUserRepository
    {
        private readonly IUserStorage storage;
        private readonly IMode mode;

        public UserRepository(IUserStorage storage, IMode mode)
        {
            if(mode.IsActivated)
                throw new ArgumentException();
            this.mode = mode;
            this.storage = storage;
        }

        public int Add(UserEntity user)
        {
            mode.Add();
            return storage.Add(user.ToUser());
        }

        public void Delete(UserEntity user)
        {
            mode.Delete();
            storage.Delete(user.ToUser());
        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            return storage.GetAll().Select(user=>user.ToUserEntity());
        }

        public int[] SearchForUsers(Func<UserEntity, bool> criteria)
        {
            Func<User, bool> predicate = user => criteria.Invoke(user.ToUserEntity());
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
