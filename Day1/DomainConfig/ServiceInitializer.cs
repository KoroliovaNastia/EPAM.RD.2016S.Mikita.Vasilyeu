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

namespace DomainConfig
{
    public static class ServiceInitializer
    {
        public static IEnumerable<UserService> InitializeServices()
        {
            var serviceSection = RegisterServices.GetConfig();
            Dictionary<string, string> serviceConfigurations = 
                new Dictionary<string, string>(serviceSection.ServicesItems.Count);

            for (int i = 0; i < serviceSection.ServicesItems.Count; i++)
            {
                var serviceType = serviceSection.ServicesItems[i].ServiceType;
                var serviceName = serviceSection.ServicesItems[i].ServiceName;
                serviceConfigurations[serviceName] = serviceType;
            }

            IList<UserService> services = new List<UserService>();
            foreach (var serviceConfiguration in serviceConfigurations)
            {
                var domain = AppDomain.CreateDomain(serviceConfiguration.Key, null, null);
                var type = typeof(DomainServiceLoader);
                var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
                var service = loader.LoadService(serviceConfiguration.Value);
                services.Add(service);
            }

            for (int i = 1; i < services.Count; ++i)
            {
                ((Slave)services[i].mode).Subscribe((Master)services[0].mode);
            }

            return services;
        }

    }
}
