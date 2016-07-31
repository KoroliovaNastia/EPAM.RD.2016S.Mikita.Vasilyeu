using BLL.Service;
using DomainConfig;
using DomainConfig.CustomSections.DependencyConfig;
using DomainConfig.CustomSections.ServiceConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceConfigurator
{
    public static class UserServiceInitializer
    {
        public static IEnumerable<BaseUserService> InitializeServices()
        {
            var conf = Parse();
            //var serviceConfigurations = ParseServiceConfigSection();
            var configs = new List<ServiceConfigInfo>();
            configs.Add(conf.MasterConfiguration);
            configs.AddRange(conf.SlaveConfigurations);
            //var dependencyConfiguration = ParseDependencyConfiguration();
            //var services = CreateServices(serviceConfigurations).ToList();
            var services = CreateServices(configs).ToList();

            var master = (MasterUserService)services.FirstOrDefault(s => s is MasterUserService);
            var slaves = services.OfType<SlaveUserService>().ToList();

            //if (master != null && dependencyConfiguration.SlaveConfigurations.Count != 0)
            //    master.Communicator.Connect(dependencyConfiguration.SlaveConfigurations.Select(c => c.IpEndPoint).ToList());
            //master.Communicator.Connect(serviceConfigurations.Select(c => c.IpEndPoint).ToList());
            master.Communicator.Connect(conf.SlaveConfigurations.Select(c => c.IpEndPoint).ToList());

            foreach (var slave in slaves)
            {
                slave.Communicator.RunReceiver();
                //slave.Subscribe(master);
            }

            //Console.WriteLine("SERVICES INFO: \n");
            //for (int i = 0; i < services.Count; i++)
            //{
            //    var service = services[i];
            //    Console.Write($"Service {i} : type = ");
            //    if (service is MasterUserService)
            //        Console.Write(" Master; ");
            //    else
            //    {
            //        Console.Write(" Slave; ");
            //    }
            //    Console.Write("Current Domain: " + AppDomain.CurrentDomain.FriendlyName + "; ");
            //    Console.Write("IsProxy: " + RemotingServices.IsTransparentProxy(service) + "; ");
            //    Console.WriteLine();
            //}
            //Console.WriteLine("\nPress enter to start: ");
            //Console.ReadLine();
            master.Load();

            return services;
        }

        public static DependencyConfiguration Parse()
        {
            var section = DependencyConfigSection.GetDependencySection();
            var config = new DependencyConfiguration
            {
                MasterName = section.MasterServices.MasterServiceName,
                MasterConfiguration = new ServiceConfigInfo
                {
                    Name = section.MasterServices.MasterServiceName,
                    Type = (ServiceType)Enum.Parse(typeof(ServiceType), section.MasterServices.MasterServiceType)
                }
            };
            int dependencyCount = section.MasterServices.Count;
            config.SlaveConfigurations = new List<ServiceConfigInfo>(dependencyCount);
            for (int i = 0; i < dependencyCount; i++)
            {
                var dependency = section.MasterServices[i];
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(dependency.IpAddress), dependency.Port);
                var slaveConfig = new ServiceConfigInfo
                {
                    IpEndPoint = endPoint,
                    Type = (ServiceType)Enum.Parse(typeof(ServiceType), dependency.ServiceType),
                    Name = dependency.ServiceName
                };
                config.SlaveConfigurations.Add(slaveConfig);
            }

            return config;
        }

        public static IEnumerable<ServiceConfigInfo> ParseServiceConfigSection()
        {
            var serviceSection = ServiceConfigSection.GetConfig();
            var serviceConfigurations = new List<ServiceConfigInfo>(serviceSection.ServicesItems.Count);

            for (int i = 0; i < serviceSection.ServicesItems.Count; i++)
            {
                var serviceType = serviceSection.ServicesItems[i].ServiceType;
                var type = serviceType.ToLower() == "master" ? ServiceType.Master : ServiceType.Slave;
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
                //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(address), port);

                serviceConfigurations.Add(new ServiceConfigInfo
                {
                    Name = serviceName,
                    Type = type,
                    IpEndPoint = endPoint
                });
            }

            return serviceConfigurations;
        }

        public static DependencyConfiguration ParseDependencyConfiguration()
        {
            var section = DependencyConfigSection.GetDependencySection();
            var config = new DependencyConfiguration
            {
                MasterName = section.MasterServices.MasterServiceName
            };
            int dependencyCount = section.MasterServices.Count;
            config.SlaveConfigurations = new List<ServiceConfigInfo>(dependencyCount);
            for (int i = 0; i < dependencyCount; i++)
            {
                var dependency = section.MasterServices[i];
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(dependency.IpAddress), dependency.Port);
                var slaveConfig = new ServiceConfigInfo
                {
                    IpEndPoint = endPoint,
                    Name = dependency.ServiceName
                };
                config.SlaveConfigurations.Add(slaveConfig);
            }

            return config;
        }

        public static IEnumerable<BaseUserService> CreateServices(IEnumerable<ServiceConfigInfo> configurations)
        {
            var services = new List<BaseUserService>();
            foreach (var configuration in configurations)
            {
                var domain = AppDomain.CreateDomain(configuration.Name, null, null);
                var type = typeof(DomainServiceLoader);
                var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
                services.Add(loader.LoadService(configuration));
            }

            return services;
        }
    }
}
