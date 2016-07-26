using BLL;
using DAL;
using DAL.Interface;
using System;
using System.Diagnostics;
using System.Linq;
using BLL.Models;
using BLL.Mappers;

namespace BLL.Service
{
    public class MasterUserService : BaseUserService
    {
        public event EventHandler<UserDataApdatedEventArgs> Deleted;
        public event EventHandler<UserDataApdatedEventArgs> Added;

        public MasterUserService(IUserRepository repository) : base(repository) { }
        public MasterUserService() : this(new UserRepository()) { }


        protected override int NotifyAdd(BllUser user)
        {
            storage.Add(user.ToDalUser());
            OnUserAdded(this, new UserDataApdatedEventArgs { User = user });
            return user.Id;
        }

        protected override void NotifyDelete(BllUser user)
        {
            storage.Delete(user.ToDalUser());
            OnUserDeleted(this, new UserDataApdatedEventArgs { User = user });
        }

        public override void Save()
        {
            storage.Save();
        }

        public override void Load()
        {
            storage.Load();
        }

        protected virtual void OnUserDeleted(object sender, UserDataApdatedEventArgs args)
        {
            Communicator?.SendDelete(args);
            Deleted?.Invoke(sender, args);
        }

        protected virtual void OnUserAdded(object sender, UserDataApdatedEventArgs args)
        {
            Communicator?.SendAdd(args);
            Added?.Invoke(sender, args);
        }

    }
}
