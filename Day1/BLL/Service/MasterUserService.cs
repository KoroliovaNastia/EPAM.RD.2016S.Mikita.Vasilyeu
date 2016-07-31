using BLL;
using DAL;
using DAL.Interface;
using System;
using System.Diagnostics;
using System.Linq;
using BLL.Models;
using BLL.Mappers;
using System.Collections.Generic;

namespace BLL.Service
{
    public class MasterUserService : BaseUserService
    {
        //public event EventHandler<UserEventArgs> Deleted;
        //public event EventHandler<UserEventArgs> Added;

        public MasterUserService(IUserRepository repository) : base(repository) { }
        public MasterUserService() : this(new UserRepository()) { }


        protected override int NotifyAdd(BllUser user)
        {
            storage.Add(user.ToDalUser());
            OnUserAdded(this, new UserEventArgs { User = user });
            return user.Id;
        }

        protected override void NotifyDelete(BllUser user)
        {
            storage.Delete(user.ToDalUser());
            OnUserDeleted(this, new UserEventArgs { User = user });
        }

        public override void Save()
        {
            storage.Save();
        }

        public override void Load()
        {
            //storage.Load();
            List<BllUser> users = storage.LoadUsers().Select(user => user.ToBllUser()).ToList();
            foreach (var user in users)
                NotifyAdd(user);

        }

        protected virtual void OnUserDeleted(object sender, UserEventArgs args)
        {
            Communicator?.SendDelete(args);
            //Deleted?.Invoke(sender, args);
        }

        protected virtual void OnUserAdded(object sender, UserEventArgs args)
        {
            Communicator?.SendAdd(args);
            //Added?.Invoke(sender, args);
        }
    }
}
