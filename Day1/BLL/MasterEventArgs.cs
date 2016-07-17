using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MasterEventArgs : EventArgs
    {
        public string Message { get; }

        public MasterEventArgs(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException(nameof(message));
            Message = message;
        }
    }
}
