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

namespace ConsoleTests
{
    class Program
    {
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

            IList<UserService> services = ServiceInitializer.InitializeServices().ToList();
            var master = services[0];
            var slave = services[1];
            master.Add(user_1);
            master.Add(user_2);
            master.Save();
            slave.Load();
            foreach (var item in slave.GetAllUsers())
            {
                Console.WriteLine(item);
            }

            for (int i = 0; i < services.Count; ++i)
            {
                var service = services[i];
                Console.Write($"Service {i} : type = ");
                Console.WriteLine("Current Domain: " + AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine("IsProxy: " + RemotingServices.IsTransparentProxy(service));
                RealProxy rp = RemotingServices.GetRealProxy(services[i]);
                int id = (int)rp.GetType().GetField("_domainID", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(rp);
                Console.WriteLine($"Id = {id}");
            }


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
