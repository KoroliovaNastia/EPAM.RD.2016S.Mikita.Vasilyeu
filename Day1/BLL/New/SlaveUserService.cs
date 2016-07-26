using BLL;
using BLL.Mappers;
using BLL.Models;
using DAL;
using DAL.Interface;
using System;
using System.Diagnostics;

namespace BLL
{
    public class SlaveUserService : BaseUserService
    {
        public SlaveUserService(IUserRepository repository) 
            : base(repository)
        {
        }
        public SlaveUserService() 
                        : this(new UserRepository())
        {

        }
        protected override int AddStrategy(BllUser user)
        {
            throw new NotSupportedException();
        }

        protected override void DeleteStrategy(BllUser user)
        {
            throw new NotSupportedException();
        }

        public override void Save()
        {
            throw new NotSupportedException();
        }

        public override void Initialize()
        {
            throw new NotSupportedException();
        }

        private void OnAdded(object sender, UserDataApdatedEventArgs args)
        {

                //Debug.WriteLine("On Added! " + AppDomain.CurrentDomain.FriendlyName);
                storage.Add(args.User.ToDalUser());
                //LastGeneratedId = args.User.Id;
        }

        private void OnDeleted(object sender, UserDataApdatedEventArgs args)
        {
            storage.Delete(args.User.ToDalUser());
        }

        public void Subscribe(MasterUserService master)
        {
            master.Deleted += OnDeleted;
            master.Added += OnAdded;
        }

        public override void AddCommunicator(UserServiceCommunicator communicator)
        {
            base.AddCommunicator(communicator);

            Communicator.UserAdded += OnAdded;
            Communicator.UserDeleted += OnDeleted;
        }
    }
}