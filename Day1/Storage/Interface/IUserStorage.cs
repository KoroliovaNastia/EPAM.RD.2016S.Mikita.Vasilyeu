using Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage.Interface
{
    public abstract class AbstractUserStorage
    {
        public List<User> Users { get; private set; }

        public AbstractUserStorage()
        {
            Users = new List<User>();
        }
    }
}
