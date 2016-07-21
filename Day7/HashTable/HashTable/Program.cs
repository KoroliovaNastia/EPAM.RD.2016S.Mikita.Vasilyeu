using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashTable
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable ht = new Hashtable();
            
            ht.Add("USA", "Washington");
            ht.Add("Germany", DateTime.Now);
            ht.Add("Sweden", new object().GetType());

            Console.WriteLine(ht["USA"].GetType());
            Console.WriteLine(ht["Germany"].GetType());
            Console.WriteLine(ht["Sweden"].GetType());
        }
    }
}
