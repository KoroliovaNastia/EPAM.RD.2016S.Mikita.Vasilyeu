using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig.CustomSections.DependencyConfig
{
    public class DependencyConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("MasterService")]
        public MasterServiceCollection MasterServices => (MasterServiceCollection) base["MasterService"];

        public static DependencyConfigSection GetDependencySection()
        {
            return (DependencyConfigSection)ConfigurationManager.GetSection("MasterDependencies");
        }
    }


    
}
