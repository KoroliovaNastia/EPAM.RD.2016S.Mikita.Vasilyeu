using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using System.Configuration;
using DomainConfig;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Reflection;
using System.Threading;
using BLL.Service;
using ServiceConfigurator;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<BaseUserService> services = UserServiceInitializer.InitializeServices().ToList();
            Console.Clear();
            Console.WriteLine("=========== Welcome to Console App ===========");
            //master.Initialize();
            var master = services.FirstOrDefault(s => s is MasterUserService);
            if (master != null)
            {
                AddSomeMasterThreads((MasterUserService)master);
            }
            string cmd = String.Empty;
            int requiredNumber = 0;
            while (cmd != "exit")
            {
                PrintServiceList(services);
                Console.WriteLine("Enter word 'service' and than type number'(service 1)");
                cmd = Console.ReadLine();
                var words = cmd.Split();
                bool parsed = false;

                if (cmd == "exit")
                {

                    return;
                    //master.Add(AnotherUser);
                }
                if (cmd == "stop")
                {
                    var slave = services.First(s => s is SlaveUserService);
                    slave.Communicator.StopReceiver();
                }
                //if (words.Length > 1)
                //{
                //    parsed = Int32.TryParse(words.Skip(1).First(), out requiredNumber);
                //}
                //if (parsed)
                //{
                //    //here must be some awesome code
                //}
            }
        }

        private static void AddSomeMasterThreads(MasterUserService master)
        {
            Random rand = new Random();
            ThreadStart masterSearch = () =>
            {

                while (true)
                {
                    var serachresult = master.SearchForUsers(new Func<BllUser, bool>(u => u.FirstName != null));
                    Console.Write("Another master thread search result: ");
                    foreach (var result in serachresult)
                    {
                        Console.Write(result + " ");
                    }
                    Console.WriteLine();
                    Thread.Sleep((int)(rand.NextDouble() * 5000));
                }

            };
            ThreadStart masterAdd = () =>
            {
                var uniqueUser = new BllUser
                {
                    LastName = "Smith",
                    FirstName = "Bob",
                    BirthDate = DateTime.Now

                };
                while (true)
                {
                    master.Add(uniqueUser);
                    Thread.Sleep((int)(rand.NextDouble() * 5000));
                    master.Delete(uniqueUser);
                    Thread.Sleep((int)(rand.NextDouble() * 5000));
                }
            };
            Thread masterSearchThread = new Thread(masterSearch);
            Thread masterSearchThread2 = new Thread(masterSearch);
            Thread masterAddThread = new Thread(masterAdd) { IsBackground = true };
            masterSearchThread.IsBackground = true;
            masterSearchThread2.IsBackground = true;
            masterAddThread.Start();
            masterSearchThread.Start();
            masterSearchThread2.Start();

        }

        private static void PrintServiceList(IEnumerable<BaseUserService> services)
        {
            var userServices = services as IList<BaseUserService> ?? services.ToList();
            int serviceCount = userServices.Count();
            Console.WriteLine("\n======== Service List ========");
            for (int i = 0; i < serviceCount; i++)
            {
                var service = userServices[i];
                Console.Write($"Service {i} : type = ");
                if (service is MasterUserService)
                    Console.WriteLine(" Master");
                else
                {
                    Console.WriteLine(" Slave");
                }
                Console.WriteLine("Current Domain: " + AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine("IsProxy: " + RemotingServices.IsTransparentProxy(service));
                var predicates = new Func<BllUser, bool>(p => p.LastName != null);
                Console.Write("User's IDs: ");
                foreach (var user in service.SearchForUsers(predicates))
                {
                    Console.Write(user + " ");
                }
                Console.WriteLine("\n" + string.Concat(Enumerable.Repeat("-", 20)));
            }
        }
    }
}
