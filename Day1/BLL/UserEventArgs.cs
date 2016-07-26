using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    [Serializable]
    public class UserEventArgs : EventArgs
    {
        public BllUser User { get; set; }
    }
}
