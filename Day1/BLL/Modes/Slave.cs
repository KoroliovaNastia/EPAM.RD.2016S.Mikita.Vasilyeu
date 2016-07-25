using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;
using System.Configuration;

namespace BLL.Modes
{
    [Serializable]
    public class Slave : MarshalByRefObject, IMode
    {
        public UserServiceCommunicator Communicator { get; set; }
        public void AddCommunicator(UserServiceCommunicator communicator)
        {
            if (communicator == null)
                return;
            Communicator = communicator;

            Communicator.UserAdded += OnAdded;
            Communicator.UserDeleted += OnDeleted;
        }

        private void OnAdded(object sender, UserDataApdatedEventArgs args)
        {

            //Debug.WriteLine("On Added! " + AppDomain.CurrentDomain.FriendlyName);
            //Repository.Add(args.User);
            //LastGeneratedId = args.User.Id;
        }

        private void OnDeleted(object sender, UserDataApdatedEventArgs args)
        {
            //Repository.Delete(args.User);
        }

        public void Subscribe(IMode mode)
        {
            Master master = mode as Master;
            if (master == null)
                throw new InvalidOperationException("Mode must be master");
            master.Added += OnChange;
            master.Deleted += OnChange;
            master.Saved += OnChange;

            master.NewAdded += OnAdded;
            master.NewDeleted += OnDeleted;
        }



        public bool IsActivated { get; private set; }

        public Slave()
        {
            IsActivated = false;
        }

        public void AddNotify()
        {
            throw new NotSupportedException();
        }

        public void DeleteNotify()
        {
            throw new NotSupportedException();
        }

        public void SaveNotify()
        {
            throw new NotSupportedException();
        }

        public void Activate()
        {
            IsActivated = true;
        }

        private void OnChange(object sender, MasterEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
