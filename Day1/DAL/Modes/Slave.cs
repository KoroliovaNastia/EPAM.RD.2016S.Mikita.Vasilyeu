using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface;

namespace DAL.Modes
{
    public class Slave : IMode
    {
        public Slave()
        {
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

    }
}
