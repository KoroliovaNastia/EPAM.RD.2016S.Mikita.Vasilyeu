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
using BLL.Interface;
using BLL.Service;

namespace DomainConfig
{
    public static class ServiceInitializer
    {
        public static IEnumerable<BaseUserService> InitializeServices()
        {
            var serviceConfigurations = GetAppConfig().ToArray();
            var services = UserServiceCreator.CreateServices(serviceConfigurations).ToList();
            var master = services.OfType<MasterUserService>().Single();
            var slaves = services.OfType<SlaveUserService>().ToList();
            ThreadInitializer.InitializeThreads(master, slaves);
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

    }
}
