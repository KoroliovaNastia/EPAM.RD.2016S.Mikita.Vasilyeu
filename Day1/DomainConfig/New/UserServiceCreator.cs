using BLL.Service;
using DomainConfig;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceConfigurator
{
    public static class UserServiceCreator
    {
        public static IEnumerable<BaseUserService> CreateServices(IEnumerable<ServiceConfigInfo> configurations)
        {
            var serviceConfigurations = configurations as ServiceConfigInfo[] ?? configurations.ToArray();

            bool namesAreUnique = CheckUniqueName(serviceConfigurations);
            if(!namesAreUnique)
                throw new ConfigurationErrorsException("Service's names must be unique!");

            bool validNetworkConfiguration = CheckNetworkConfiguration(serviceConfigurations);
            if(!validNetworkConfiguration)
                throw new ConfigurationErrorsException("Some of services don't have ip Address");

            return serviceConfigurations.Select(CreateService).ToList();
        }

        private static BaseUserService CreateService(ServiceConfigInfo configuration)
        {
            
            var domain = AppDomain.CreateDomain(configuration.Name, null, null);
            var type = typeof(DomainServiceLoader);
            var loader = (DomainServiceLoader)domain.CreateInstanceAndUnwrap(Assembly.GetAssembly(type).FullName, type.FullName);
           // Console.WriteLine("Creating service " + configuration.Name);
            var assemblies = domain.GetAssemblies();
            //Console.WriteLine("Assemblies: ");
            //foreach (var assembly in assemblies)
            //{
            //    Console.WriteLine(assembly.FullName);
            //}
            //Console.WriteLine(RemotingServices.IsTransparentProxy(loader));
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Task1.StorageSystem.dll");
            return loader.LoadService(configuration);
        }

        private static bool CheckNetworkConfiguration(IEnumerable<ServiceConfigInfo> configuration)
        {
            var slavesConfig = configuration.Where(c => c.Type == ServiceType.Slave).ToList();
            return slavesConfig.All(serviceConfiguration => serviceConfiguration.IpEndPoint?.Address != null);
        }

        private static bool CheckUniqueName(IEnumerable<ServiceConfigInfo> configurations)
        {
            var configurationNames = configurations.Select(c => c.Name).ToList();

            return configurationNames.Distinct().Count() == configurationNames.Count;
        }

    }
}
