using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interface
{
    public interface ICustomerEnumerator : IEnumerator<int>
    {
        int GetNext();
        void SetCurrent(int seed);
    }
}
