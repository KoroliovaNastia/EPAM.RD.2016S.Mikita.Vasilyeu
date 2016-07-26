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

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<BaseUserService> services = ServiceFactory.InitializeServices().ToList();

            while(true)
            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];
                Console.Write($"Service {i} : type = ");
                if (service is MasterUserService)
                    Console.WriteLine(" Master");
                else
                    Console.WriteLine(" Slave");
                Console.WriteLine("Current Domain: " + AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine("IsProxy: " + RemotingServices.IsTransparentProxy(service));
                Console.Write("User's IDs: ");
                foreach (var user in service.SearchForUsers(user => !string.IsNullOrWhiteSpace(user.FirstName)))
                    Console.Write(user + " ");
                Console.WriteLine("\n\n");
                Thread.Sleep(5000);
            }
        }
    }
}
