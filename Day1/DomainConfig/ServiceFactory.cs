using BLL.Service;
using DomainConfig;
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
            var configurations = ParseServiceConfigSection();
            var services = CreateServices(configurations.MasterConfiguration, configurations.SlaveConfigurations).ToList();
            var master = (MasterUserService)services.FirstOrDefault(s => s is MasterUserService);
            var slaves = services.OfType<SlaveUserService>().ToList();

            if (master != null && configurations.SlaveConfigurations.Count != 0)
                master.Communicator.Connect(configurations.SlaveConfigurations.Select(c => c.IpEndPoint).ToList());

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

        public static DependencyConfigInfo ParseServiceConfigSection()
        {
            var section = ServiceConfigSection.GetSection();
            var config = new DependencyConfigInfo
            {
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

        private static IEnumerable<BaseUserService> CreateServices(ServiceConfigInfo masterConfig, IEnumerable<ServiceConfigInfo> slaveConfigs)
        {
            var services = new List<BaseUserService>();
            services.Add(CreateService(masterConfig));
            foreach (var configuration in slaveConfigs)
                services.Add(CreateService(configuration));

            return services;
        }

        private static BaseUserService CreateService(ServiceConfigInfo configuration)
        {
            var domain = AppDomain.CreateDomain(configuration.Name, null, null);
            var type = typeof(DomainServiceLoader);
            var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);

            return loader.LoadService(configuration);
        }
    }
}
