using BLL;
using BLL.Mappers;
using BLL.Models;
using DAL;
using DAL.Interface;
using System;
using System.Diagnostics;

namespace BLL.Service
{
    public class SlaveUserService : BaseUserService
    {
        public SlaveUserService(IUserRepository repository) : base(repository) { }
        public SlaveUserService() : this(new UserRepository()) { }

        protected override int NotifyAdd(BllUser user)
        {
            throw new NotSupportedException();
        }

        protected override void NotifyDelete(BllUser user)
        {
            throw new NotSupportedException();
        }

        public override void Save()
        {
            throw new NotSupportedException();
        }

        public override void Load()
        {
            throw new NotSupportedException();
        }

        private void OnAdded(object sender, UserEventArgs args)
        {
            storageLock.EnterWriteLock();
            try
            {
                storage.Add(args.User.ToDalUser());
            }
            finally
            {
                storageLock.ExitWriteLock();
            }
        }

        private void OnDeleted(object sender, UserEventArgs args)
        {
            storageLock.EnterWriteLock();
            try
            {
                storage.Delete(args.User.ToDalUser());
            }
            finally
            {
                storageLock.ExitWriteLock();
            }
        }

        public void Subscribe(MasterUserService master)
        {
            master.Added += OnAdded;
            master.Deleted += OnDeleted;
        }

        public override void AddCommunicator(UserServiceCommunicator communicator)
        {
            base.AddCommunicator(communicator);
            Communicator.UserAdded += OnAdded;
            Communicator.UserDeleted += OnDeleted;
        }
    }
}