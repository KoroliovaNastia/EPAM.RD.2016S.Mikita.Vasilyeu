using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig.CustomSections.ServiceConfig
{
    public class ServiceConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("MasterService")]
        public MasterServiceCollection MasterServices => (MasterServiceCollection) base["MasterService"];

        public static ServiceConfigSection GetSection()
        {
            return (ServiceConfigSection)ConfigurationManager.GetSection("ServiceConfigSection");
        }
    }
}
