using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;
using DAL.Interface;

namespace BLL.Modes
{
    [Serializable]
    public class Master : MarshalByRefObject, IMode
    {
        public IUserRepository Repository { get; set; }

        public event EventHandler<UserEventArgs> NewDeleted;
        public event EventHandler<UserEventArgs> NewAdded;
        public UserServiceCommunicator Communicator { get; set; }

        public void AddCommunicator(UserServiceCommunicator communicator)
        {
            if (communicator == null)
                return;
            Communicator = communicator;
        }

        protected virtual void OnUserDeleted(object sender, UserEventArgs args)
        {
            Communicator?.SendDelete(args);
            NewDeleted?.Invoke(sender, args);
        }

        protected virtual void OnUserAdded(object sender, UserEventArgs args)
        {
            Communicator?.SendAdd(args);
            NewAdded?.Invoke(sender, args);
        }


        private static Master instance;
        private static readonly object syncRoot = new object();

        public event EventHandler<MasterEventArgs> Added;
        public event EventHandler<MasterEventArgs> Deleted;
        public event EventHandler<MasterEventArgs> Saved;

        public bool IsActivated { get; private set; }

        private Master()
        {
            IsActivated = false;
            instance = null;
        }

        public static Master Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Master();
                        }
                    }
                }
                return instance;
            }
        }

        public void AddNotify()
        {
            Added?.Invoke(this, new MasterEventArgs("Added!"));
        }

        public void DeleteNotify()
        {
            Deleted?.Invoke(this, new MasterEventArgs("Deleted!"));
        }

        public void SaveNotify()
        {
            Saved?.Invoke(this, new MasterEventArgs("Saved!"));
        }

        public void Activate()
        {
            IsActivated = true;
        }

        public void Subscribe(IMode mode)
        {
            throw new NotSupportedException();
        }
    }
}
