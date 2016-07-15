using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;

namespace DAL.Modes
{
    public class Master : IMode
    {
        public event EventHandler<MasterEventArgs> Added;
        public event EventHandler<MasterEventArgs> Deleted;

        private static Master instance;

        private Master() { }

        public static Master Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Master();
                }
                return instance;
            }
        }

        public void Add()
        {
            Added?.Invoke(this, new MasterEventArgs("Added!"));
        }

        public void Delete()
        {
            Deleted?.Invoke(this, new MasterEventArgs("Deleted!"));
        }

        
    }
}
