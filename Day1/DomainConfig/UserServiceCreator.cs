using BLL;
using BLL.Modes;
using BLL.Service;
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
        public static IEnumerable<BaseUserService> CreateServices(IEnumerable<ServiceConfigInfo> configurations)
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
    }
    
}
