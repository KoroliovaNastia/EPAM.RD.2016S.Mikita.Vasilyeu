using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig
{
    public class DependencyConfigInfo
    {
        public ServiceConfigInfo MasterConfiguration { get; set; }
        public IList<ServiceConfigInfo> SlaveConfigurations { get; set; }
    }
}
