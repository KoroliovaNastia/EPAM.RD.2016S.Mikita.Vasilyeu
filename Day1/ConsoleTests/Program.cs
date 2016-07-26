using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Models;
using BLL.Modes;
using System.Configuration;
using DomainConfig;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Proxies;
using System.Reflection;
using System.Threading;

namespace ConsoleTests
{
    class Program
    {
        public static void PrintServiceList(IEnumerable<BaseUserService> services)
        {
            var userServices = services as IList<BaseUserService> ?? services.ToList();
            int serviceCount = userServices.Count();
            Console.WriteLine("\n======== Service List ========");
            for (int i = 0; i < serviceCount; i++)
            {
                var service = userServices[i];
                Console.Write($"Service {i} : type = ");
                //if (service is MasterUserService)
                //    Console.WriteLine(" Master");
                //else
                //{
                //    Console.WriteLine(" Slave");
                //}
                Console.WriteLine("Current Domain: " + AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine("IsProxy: " + RemotingServices.IsTransparentProxy(service));
                var predicates = new Func<BllUser, bool>[] { p => p.LastName != null };
                Console.Write("User's IDs: ");
                if (service.SearchForUsers(predicates) != null)
                    foreach (var user in service.SearchForUsers(predicates))
                    {
                        Console.Write(user + " ");
                    }
                Console.WriteLine("\n" + string.Concat(Enumerable.Repeat("-", 20)));
                Thread.Sleep(2000);
            }
        }


        static void Main(string[] args)
        {
            BllUser user_1 = new BllUser
            {
                FirstName = "Mike",
                LastName = "Jones",
                Cards =
                {
                    new BllVisaRecord { Country = "USA" },
                    new BllVisaRecord { Country = "China" }
                }
            };
            BllUser user_2 = new BllUser
            {
                FirstName = "Mike",
                LastName = "Smith"
            };

            IList<BaseUserService> services = ServiceInitializer.InitializeServices().ToList();
            while (true)
                PrintServiceList(services);
            //var master = services[0];
            //var slave = services[1];
            //master.Add(user_1);
            //master.Add(user_2);
            //master.Save();
            //slave.Load();
            //foreach (var item in slave.GetAllUsers())
            //{
            //    Console.WriteLine(item);
            //}

            //for (int i = 0; i < services.Count; ++i)
            //{
            //    var service = services[i];
            //    Console.Write($"Service {i} : type = ");
            //    Console.WriteLine("Current Domain: " + AppDomain.CurrentDomain.FriendlyName);
            //    Console.WriteLine("IsProxy: " + RemotingServices.IsTransparentProxy(service));
            //    RealProxy rp = RemotingServices.GetRealProxy(services[i]);
            //    int id = (int)rp.GetType().GetField("_domainID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(rp);
            //    Console.WriteLine($"Id = {id}");
            //}


            //UserService repositoryMaster = new UserService();
            //UserService repositorySlave = new UserService(new Slave());
            //BllUser user_1 = new BllUser
            //{
            //    FirstName = "Mike",
            //    LastName = "Jones",
            //    Cards =
            //    {
            //        new BllVisaRecord { Country = "USA" },
            //        new BllVisaRecord { Country = "China" }
            //    }
            //};
            //BllUser user_2 = new BllUser { FirstName = "Mike", LastName = "Smith" };
            //repositoryMaster.Add(user_1);
            //repositoryMaster.Add(user_2);
            //repositoryMaster.Save();
            //repositorySlave.Load();
            //foreach (var item in repositorySlave.GetAllUsers())
            //{
            //    Console.WriteLine(item);
            //}
        }
    }
}
