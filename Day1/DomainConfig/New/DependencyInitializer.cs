using System.Collections.Generic;
using System.Linq;
using System.Net;
using BLL.Service;
using DomainConfig;

namespace ServiceConfigurator
{
    public class DependencyInitializer
    {
        public static void InitalizeDependencies(MasterUserService master, DependencyConfiguration configuration)
        {
            if(master == null)
                return;
            //if(master.Name != configuration.MasterName)
            //    return;
            if(configuration.SlaveConfigurations.Count == 0)
                return;
            master.Communicator.Connect(configuration.SlaveConfigurations.Select(c => c.IpEndPoint).ToList());
        }
    }
}