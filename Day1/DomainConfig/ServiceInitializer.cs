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
            var serviceConfigurations = ParseAppConfig();       
            IList<UserService> services = new List<UserService>();
            var configurations = serviceConfigurations as ServiceConfigInfo[] ?? serviceConfigurations.ToArray();
            services = UserServiceCreator.CreateServices(configurations).ToList();

            var master = services.FirstOrDefault(s => s.Mode is Master);

            if (master == null)
            {
                throw new ConfigurationErrorsException("Master is not exist");
            }

            var slaves = services.Where(s => s.Mode is Slave);
            //SubscribeServices(master, slaves);
            ThreadInitializer.InitializeThreads(master, slaves);
            return services;
        }

        private static IEnumerable<ServiceConfigInfo> ParseAppConfig()
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

    }
}
