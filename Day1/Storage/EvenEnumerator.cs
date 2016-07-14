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
        private int next;

        public EvenEnumerator(int seed = -2)
        {
            next = seed;
        }

        public int Current => next;

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
