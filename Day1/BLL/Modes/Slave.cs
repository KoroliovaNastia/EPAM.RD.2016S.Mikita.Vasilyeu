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
    public class Slave : IMode
    {
        public static int Counter { get; private set; }
        public bool IsActivated { get; private set; } = false;

        public Slave()
        {
            if(++Counter> int.Parse(ConfigurationManager.AppSettings["SlavesNumber"]))
                throw new ArgumentException();
            Master.Instance.Added += OnChange;
            Master.Instance.Deleted += OnChange;
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        private void OnChange(object sender, MasterEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        public void Activate()
        {
            IsActivated = true;
        }
    }
}
