using BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainConfig.CustomConfigSections;
using System.Reflection;
using System.Net;
using BLL.Service;
using System.Threading;
using BLL.Models;

namespace DomainConfig
{
    public static class ServiceFactory
    {
        public static IEnumerable<BaseUserService> InitializeServices()
        {
            var serviceConfigurations = GetAppConfig().ToArray();
            var services = CreateServices(serviceConfigurations).ToList();
            var master = services.OfType<MasterUserService>().Single();
            var slaves = services.OfType<SlaveUserService>().ToList();
            InitializeThreads(master, slaves);
            return services;
        }

        private static IEnumerable<ServiceConfigInfo> GetAppConfig()
        {
            var serviceSection = RegisterServices.GetConfig();
            IList<ServiceConfigInfo> serviceConfigurations =
                    new List<ServiceConfigInfo>(serviceSection.ServicesItems.Count);

            for (int i = 0; i < serviceSection.ServicesItems.Count; i++)
            {
                var serviceType = serviceSection.ServicesItems[i].ServiceType;
                ServiceType type = serviceType.ToLower() == "master" ? ServiceType.Master : ServiceType.Slave;
                var serviceName = serviceSection.ServicesItems[i].ServiceName;
                var address = serviceSection.ServicesItems[i].IpAddress;
                int port = serviceSection.ServicesItems[i].Port;
                IPAddress ipAddress;
                bool parsed = IPAddress.TryParse(address, out ipAddress);
                IPEndPoint endPoint = null;
                if (parsed)
                {
                    endPoint = new IPEndPoint(IPAddress.Parse(address), port);
                }

                serviceConfigurations.Add(new ServiceConfigInfo
                {
                    Name = serviceName,
                    Type = type,
                    IpEndPoint = endPoint
                });
            }

            return serviceConfigurations;
        }

        private static IEnumerable<BaseUserService> CreateServices(IEnumerable<ServiceConfigInfo> configurations)
        {
            var serviceConfigurations = configurations.ToArray();
            var services = new List<BaseUserService>();
            MasterUserService master = null;
            DomainServiceLoader masterLoader = null;

            foreach (var serviceConfiguration in serviceConfigurations)
            {
                var domain = AppDomain.CreateDomain(serviceConfiguration.Name, null, null);
                var type = typeof(DomainServiceLoader);
                var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
                var service = loader.LoadService(serviceConfiguration);
                var userService = service as MasterUserService;
                if (userService != null)
                {
                    master = userService;
                    masterLoader = loader;
                }
                services.Add(service);
            }

            if (master != null)
                masterLoader.ConnectMaster(master, serviceConfigurations.Where(s => s.Type == ServiceType.Slave).ToList());

            return services;
        }

        private static void InitializeThreads(MasterUserService master, IEnumerable<SlaveUserService> slaves)
        {
            var users = new List<BllUser>
            {
                new BllUser
                {
                    FirstName = "Mike",
                    LastName = "Jones",
                    Cards =
                    {
                        new BllVisaRecord {Country = "USA"},
                        new BllVisaRecord {Country = "China"}
                    }
                },
                new BllUser
                {
                    FirstName = "Mike",
                    LastName = "Smith",
                    Cards =
                    {
                        new BllVisaRecord {Country = "Germany"},
                        new BllVisaRecord {Country = "Sweden"}
                    }
                },
                new BllUser
                {
                    FirstName = "Kelly",
                    LastName = "Mitchell",
                    Cards =
                    {
                        new BllVisaRecord {Country = "PRC"},
                        new BllVisaRecord {Country = "USSR"}
                    }
                }
            };
            Func<BllUser, bool> criteria = user => !string.IsNullOrWhiteSpace(user.FirstName);
            var masterThread = new Thread(() =>
            {
                BllUser previousUser = null;
                while (true)
                {
                    foreach (var user in users)
                    {
                        master.Add(user);
                        Thread.Sleep(5000);
                        if (previousUser != null)
                        {
                            master.Delete(previousUser);
                        }
                        previousUser = user;
                        Thread.Sleep(5000);
                        Console.WriteLine("\nSearch in master:");
                        var userIds = master.SearchForUsers(criteria);
                        Console.Write("User's IDs in master: ");
                        foreach (var id in userIds)
                            Console.Write(id + " ");
                        Console.WriteLine("\n");
                    }
                }
            });
            masterThread.IsBackground = true;
            masterThread.Start();

            foreach (var slave in slaves)
            {
                var slaveThread = new Thread(() =>
                {
                    while (true)
                    {
                        var userIds = slave.SearchForUsers(criteria);
                        Console.WriteLine();
                        Console.Write("\nUser's IDs in slave: ");
                        foreach (var id in userIds)
                            Console.Write(id + " ");
                        Console.WriteLine();
                        Thread.Sleep(5000);
                    }

                });
                slaveThread.Start();
                slaveThread.IsBackground = true;
            }
        }

    }
}
