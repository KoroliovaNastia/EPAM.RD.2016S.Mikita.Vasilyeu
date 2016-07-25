using BLL;
using BLL.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig
{
    public static class UserServiceCreator
    {
        public static IEnumerable<UserService> CreateServices(IEnumerable<ServiceConfigInfo> configurations)
        {
            var serviceConfigurations = configurations as ServiceConfigInfo[] ?? configurations.ToArray();
            var services = new List<UserService>();
            UserService master = null;
            DomainServiceLoader masterLoader = null;

            foreach (var serviceConfiguration in serviceConfigurations)
            {
                var domain = AppDomain.CreateDomain(serviceConfiguration.Name, null, null);
                var type = typeof(DomainServiceLoader);
                var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
                var service = loader.LoadService(serviceConfiguration); // create service
                if (service.Mode is Master)
                {
                    master = service;
                    masterLoader = loader;
                }

                services.Add(service);
            }

            if (master != null)
            {
                var enumerable = serviceConfigurations.Where(c => c.Type == ServiceType.Slave).ToList();
                masterLoader.ConnectMaster(master, serviceConfigurations.Where(c => c.Type == ServiceType.Slave).ToList());
            }

            return services;
        }
    }
    
}
