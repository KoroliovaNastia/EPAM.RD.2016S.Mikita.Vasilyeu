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
            ShowServicesInfo(services);
            Console.WriteLine("\nPress enter to start: ");
            Console.ReadLine();
            var master = (MasterUserService)services.Single(s => s is MasterUserService);
            var slaves = services.Where(s => s is SlaveUserService).Select(s => (SlaveUserService)s);
            RunSlaves(slaves);
            RunMaster(master);
            while (true)
            {
                var quit = Console.ReadKey();
                if (quit.Key == ConsoleKey.Escape)
                    break;
            }
        }

        private static void RunMaster(MasterUserService master)
        {
            Random rand = new Random();

            ThreadStart masterSearch = () =>
            {
                while (true)
                {
                    var serachresult = master.SearchForUsers(u => u.FirstName != null);
                    Console.Write("Master search results: ");
                    foreach (var result in serachresult)
                        Console.Write(result + " ");
                    Console.WriteLine();
                    Thread.Sleep(rand.Next(1000, 5000));
                }
            };

            ThreadStart masterAddDelete = () =>
            {
                var users = new List<BllUser>
                {
                    new BllUser { FirstName = "Bob", LastName = "Smith"},
                    new BllUser { FirstName = "Jack", LastName = "Jackson"},
                };
                BllUser userToDelete = null;

                while (true)
                {
                    foreach (var user in users)
                    {
                        int addChance = rand.Next(0, 3);
                        if (addChance == 0)
                            master.Add(user);

                        Thread.Sleep(rand.Next(1000, 5000));
                        if (userToDelete != null)
                        {
                            int deleteChance = rand.Next(0, 3);
                            if (deleteChance == 0)
                                master.Delete(userToDelete);
                        }
                        userToDelete = user;
                        Thread.Sleep(rand.Next(1000, 5000));
                        Console.WriteLine();
                    }
                }
            };

            Thread masterSearchThread = new Thread(masterSearch) { IsBackground = true };
            Thread masterAddThread = new Thread(masterAddDelete) { IsBackground = true };
            masterAddThread.Start();
            masterSearchThread.Start();
        }

        private static void RunSlaves(IEnumerable<SlaveUserService> slaves)
        {
            Random rand = new Random();

            foreach (var slave in slaves)
            {
                var slaveThread = new Thread(() =>
                {
                    while (true)
                    {
                        var userIds = slave.SearchForUsers(u => !string.IsNullOrWhiteSpace(u.FirstName));
                        Console.Write("Slave search results: ");
                        foreach (var user in userIds)
                            Console.Write(user + " ");
                        Console.WriteLine();
                        Thread.Sleep((int)(rand.NextDouble() * 5000));
                    }

                });
                slaveThread.IsBackground = true;
                slaveThread.Start();
            }
        }

        private static void ShowServicesInfo(IEnumerable<BaseUserService> services)
        {
            var servicesList = services.ToList();
            Console.WriteLine("SERVICES INFO: \n");
            for (int i = 0; i < servicesList.Count; i++)
            {
                var service = servicesList[i];
                Console.Write($"Service {i} : type = ");
                if (service is MasterUserService)
                    Console.Write(" Master; ");
                else
                {
                    Console.Write(" Slave; ");
                }
                Console.Write("Current Domain: " + AppDomain.CurrentDomain.FriendlyName + "; ");
                Console.Write("IsProxy: " + RemotingServices.IsTransparentProxy(service) + "; ");
                Console.WriteLine();
            }
        }
    }
}
