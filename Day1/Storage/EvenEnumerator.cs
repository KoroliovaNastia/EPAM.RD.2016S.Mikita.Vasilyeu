using Storage.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public class EvenEnumerator : ICustomerEnumerator
    {
        private int next = -2;

        public int Current
        {
            get { return next; }         
        }

        public bool MoveNext()
        {
            if (next + 1 == int.MaxValue)
                return false;
            next += 2;
            return true;
        }

        public int GetNext()
        {
            if (MoveNext())
                return Current;
            return 0;
        }
    }
}
