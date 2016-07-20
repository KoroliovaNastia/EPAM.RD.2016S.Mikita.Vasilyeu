using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTests
{
    public delegate void Delegate1(int x);
    public delegate void Delegate2(int x);
    class Program
    {
 
        static void Main(string[] args)
        {
            //KeyValuePair<int, string> a = new KeyValuePair<int, string>(2, "20");
            //KeyValuePair<int, string> b = new KeyValuePair<int, string>(2, "21");

            //Delegate1 d1 = i => { };
            //Delegate2 d2 = i => { };

            //var d3 = d1;
            //var d4 = d2;
            //Console.WriteLine(d1.GetHashCode() + " " + d2.GetHashCode());
            //Console.WriteLine(d3.GetHashCode() + " " + d4.GetHashCode());
            //Console.WriteLine(a.GetHashCode() + " " + b.GetHashCode());

            string s1 = "";
            StringBuilder s2 = new StringBuilder();

            Stopwatch st1 = new Stopwatch();
            st1.Start();

            for (int i = 0; i < 10000; i++)
            {
                s1 = s1 + i.ToString();
            }
            st1.Stop();
            Console.WriteLine(st1.ElapsedTicks);
            st1.Reset();
            st1.Start();
            for (int i = 0; i < 10000; i++)
            {
                s2.Append(i.ToString());
            }

            st1.Stop();
            Console.WriteLine(st1.ElapsedTicks);
        }


    }
}
