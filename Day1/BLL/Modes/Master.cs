using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface;

namespace BLL.Modes
{
    [Serializable]
    public class Master : MarshalByRefObject, IMode
    {
        private static Master instance;
        private static object syncRoot = new object();

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
            if (Added == null)
                return;
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
    }
}
