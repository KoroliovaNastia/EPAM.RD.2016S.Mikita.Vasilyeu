using DAL.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    [Serializable]
    public class EvenEnumerator : ICustomerEnumerator
    {
        private int next;

        public EvenEnumerator(int seed = -2)
        {
            SetCurrent(seed);
        }

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

        public void SetCurrent(int seed)
        {
            if (seed < -2 || seed % 2 != 0)
                throw new ArgumentException();
            next = seed;
        }

        public void Reset()
        {
            next = -2;
        }

        object IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
