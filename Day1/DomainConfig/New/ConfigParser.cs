using DomainConfig;
using DomainConfig.CustomConfigSections;
using DomainConfig.DependenciesConfigSections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;

namespace ServiceConfigurator
{
    public class ConfigParser
    {
        public static IEnumerable<ServiceConfigInfo> ParseServiceConfigSection()
        {
            var serviceSection = RegisterServices.GetConfig();
            IList<ServiceConfigInfo> serviceConfigurations =
                    new List<ServiceConfigInfo>(serviceSection.ServicesItems.Count);

            for (int i = 0; i < serviceSection.ServicesItems.Count; i++)
            {
                var serviceType = serviceSection.ServicesItems[i].ServiceType;
                ServiceType type = serviceType.ToLower() == "master" ? ServiceType.Master : ServiceType.Slave;
                var serviceName = serviceSection.ServicesItems[i].ServiceName;
                //string filePath = GetXmlFilePath();
                //BooleanSwitch loggingSwitch = new BooleanSwitch("loggingSwitch", "Switch in config file");

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
                    //FilePath = filePath,
                    //LoggingEnabled = loggingSwitch.Enabled,
                    IpEndPoint = endPoint
                });
            }

            return serviceConfigurations;
        }

        public static DependencyConfiguration ParseDependencyConfiguration()
        {
            var section = GetDependencySection();
            var config = new DependencyConfiguration
            {
                MasterName = section.MasterServices.MasterServiceName
            };
            int dependencyCount = section.MasterServices.Count;
            config.SlaveConfigurations = new List<ServiceConfigInfo>(dependencyCount);
            for (int i = 0; i < dependencyCount; i++)
            {
                var dependency = section.MasterServices[i];
                IPAddress address;
                bool parsed = IPAddress.TryParse(dependency.IpAddress, out address);
                if(!parsed)
                    throw new ArgumentException("Address is not valid");
                var slaveConfig = new ServiceConfigInfo
                {
                    IpEndPoint =  new IPEndPoint(address, dependency.Port),
                    Name = dependency.ServiceName
                };
                config.SlaveConfigurations.Add(slaveConfig);
            }

            return config;
        }

        private static DependencyConfigSection GetDependencySection()
        {
            return (DependencyConfigSection) ConfigurationManager.GetSection("MasterDependencies");
        }

    }
}