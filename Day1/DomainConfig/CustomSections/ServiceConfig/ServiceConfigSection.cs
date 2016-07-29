using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig.CustomSections.ServiceConfig
{
    public class ServiceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("Services")]
        public ServiceCollection ServicesItems => ((ServiceCollection)(base["Services"]));

        public static ServiceConfigSection GetConfig()
        {
            return (ServiceConfigSection)ConfigurationManager.GetSection("RegisterServices") ?? new ServiceConfigSection();
        }
    }
}
