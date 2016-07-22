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

        public void Subscribe(IMode mode)
        {
            Master master = mode as Master;
            if (master == null)
                throw new InvalidOperationException("Mode must be master");
            master.Added += OnChange;
            master.Deleted += OnChange;
            master.Saved += OnChange;
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
