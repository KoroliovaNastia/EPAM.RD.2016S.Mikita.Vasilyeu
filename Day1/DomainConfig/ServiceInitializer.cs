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
using BLL.Modes;
using System.Net;

namespace DomainConfig
{
    public static class ServiceInitializer
    {
        public static IEnumerable<UserService> InitializeServices()
        {
            var serviceSection = RegisterServices.GetConfig();
            //Dictionary<string, string> serviceConfigurations =
            //    new Dictionary<string, string>(serviceSection.ServicesItems.Count);
            //for (int i = 0; i < serviceSection.ServicesItems.Count; i++)
            //{
            //    var serviceType = serviceSection.ServicesItems[i].ServiceType;
            //    var serviceName = serviceSection.ServicesItems[i].ServiceName;
            //    serviceConfigurations[serviceName] = serviceType;
            //}
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

            IList<UserService> services = new List<UserService>();
            var configurations = serviceConfigurations as ServiceConfigInfo[] ?? serviceConfigurations.ToArray();
            //services = UserServiceCreator.CreateServices(configurations).ToList();

            //Master master = null;
            UserService master = null;
            DomainServiceLoader masterLoader = null;

            foreach (var serviceConfiguration in serviceConfigurations)
            {
                var domain = AppDomain.CreateDomain(serviceConfiguration.Name, null, null);
                var type = typeof(DomainServiceLoader);
                var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
                //Console.WriteLine("Creating service " + serviceConfiguration.Name);
                //var assemblies = domain.GetAssemblies();
                //Console.WriteLine("Assemblies: ");
                //foreach (var assembly in assemblies)
                //{
                //    Debug.WriteLine(assembly.FullName);
                //}
                //var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Task1.StorageSystem.dll");

                var service = loader.LoadService(serviceConfiguration); // create service

                //var userService = service.Mode as Master;

                if (service != null)
                {
                    //master = userService;
                    master = service;
                    masterLoader = loader;
                }

                services.Add(service);
            }

            if (master != null)
            {
                //masterLoader.ConnectMaster(master, serviceConfigurations.Where(c => c.Type == ServiceType.Slave).ToList());
                master.Mode.Communicator.Connect(configurations.Where(c => c.IpEndPoint != null)
                                                           .Select(c => c.IpEndPoint));
            }

           // master = (Master)services.FirstOrDefault(s => s.Mode is Master).Mode;

            if (master == null)
            {
                throw new ConfigurationErrorsException("Master is not exist");
            }



            //var slaves = services.Where(s => s.Mode is Slave).Select(s => s.Mode);
            var slaves = services.Where(s => s.Mode is Slave);
            //SubscribeServices(master, slaves);
            ThreadInitializer.InitializeThreads(master, slaves);
            //foreach (var serviceConfiguration in serviceConfigurations)
            //{
            //    var domain = AppDomain.CreateDomain(serviceConfiguration.Key, null, null);
            //    var type = typeof(DomainServiceLoader);
            //    var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
            //    var service = loader.LoadService(serviceConfiguration.Value);
            //    services.Add(service);
            //}

            //var master = services.Single(s => s.Mode is Master).Mode;
            //var slaves = services.Where(s => s.Mode is Slave).Select(s => s.Mode);

            //foreach (var slave in slaves)
            //{
            //    slave.Subscribe(master);
            //}

            return services;
        }

    }
}
