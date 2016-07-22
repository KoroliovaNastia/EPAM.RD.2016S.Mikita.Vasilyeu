using BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainConfig.CustomConfigSections;

namespace DomainConfig
{
    public static class ServiceInitializer
    {
        public static IEnumerable<UserService> InitializeServices()
        {
            var serviceConfigurations = ParseAppConfig();
            IList<UserService> services = new List<UserService>();
            foreach (var serviceConfiguration in serviceConfigurations)
            {
                var service = UserServiceCreator.CreateService(serviceConfiguration);
                Console.WriteLine("-----Services has been created");
                services.Add(service);
            }
            //var master = (MasterUserService)services.FirstOrDefault(s => s is MasterUserService);

            //if (master == null)
            //{
            //    throw new ConfigurationErrorsException("Master is not exist");
            //}

            //var slaves = services.OfType<SlaveUserService>();
            //SubscribeServices(master, slaves);
            return services;
        }

        private static IEnumerable<ServiceConfiguration> ParseAppConfig()
        {
            var serviceSection = GetServiceSection();
            IList<ServiceConfiguration> serviceConfigurations =
                    new List<ServiceConfiguration>(serviceSection.ServicesItems.Count);

            for (int i = 0; i < serviceSection.ServicesItems.Count; i++)
            {
                var serviceType = serviceSection.ServicesItems[i].ServiceType;
                ServiceType type = serviceType.ToLower() == "master" ? ServiceType.Master : ServiceType.Slave;
                var serviceName = serviceSection.ServicesItems[i].ServiceName;
                //string filePath = FileInitializer.GetXmlFilePath();
                BooleanSwitch loggingSwitch = new BooleanSwitch("Data", "Switch in config file");

                serviceConfigurations.Add(new ServiceConfiguration
                {
                    Name = serviceName,
                    Type = type,
                    //FilePath = filePath,
                    LoggingEnabled = loggingSwitch.Enabled
                });
            }

            return serviceConfigurations;
        }

        private static RegisterServices GetServiceSection()
        {
            return (RegisterServices)ConfigurationManager.GetSection("RegisterServices");
        }
    }
}
