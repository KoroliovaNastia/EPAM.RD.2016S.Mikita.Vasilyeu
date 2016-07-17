using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICustomerEnumerator
    {
        int Current { get; set; }
        bool MoveNext();
        int GetNext();
    }
}
