using BLL.Service;
using DomainConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceConfigurator
{
    public static class UserServiceInitializer
    {
        //TODO create interface with Settings so you'll be able to test creator
        public static IEnumerable<BaseUserService> InitializeServices()
        {           
            var serviceConfigurations = ConfigParser.ParseServiceConfigSection();
            var dependencyConfugiration = ConfigParser.ParseDependencyConfiguration();
            var configurations = serviceConfigurations as IList<ServiceConfigInfo> ?? serviceConfigurations.ToList();

            IList<BaseUserService> services = UserServiceCreator.CreateServices(configurations).ToList();

            InitializeComponents(services, configurations, dependencyConfugiration);

            return services;     
        }

        private static void InitializeComponents(IEnumerable<BaseUserService> services,
            IEnumerable<ServiceConfigInfo> configurations, DependencyConfiguration dependencyConfiguration)
        {
            var userServices = services as IList<BaseUserService> ?? services.ToList();

            var master = (MasterUserService)userServices.FirstOrDefault(s => s is MasterUserService);

            //if (master == null)
            //{
            //    throw new ConfigurationErrorsException("Master is not exist");
            //}

            var slaves = userServices.OfType<SlaveUserService>().ToList();

            var slavesAddresses = configurations.Where(c => c.Type == ServiceType.Slave)
                                                .Select(c => c.IpEndPoint)
                                                .ToList();

            //master.Communicator.ConnectGroup(slavesAddresses);
            DependencyInitializer.InitalizeDependencies(master, dependencyConfiguration);
            foreach (var slaveUserService in slaves)
            {
                slaveUserService.Communicator.RunReceiver();
            }

            //SubscribeServices(master, slaves);
            ThreadInitializer.InitializeThreads(master, slaves);
        }
        private static void SubscribeServices(MasterUserService master, IEnumerable<SlaveUserService> slaves)
        {
            foreach (var slave in slaves)
            {
                slave.Subscribe(master);
            }
        }
    }
}
