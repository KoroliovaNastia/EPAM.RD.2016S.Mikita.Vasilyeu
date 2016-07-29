using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig.CustomSections.ServiceConfig
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("serviceType", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string ServiceType
        {
            get { return (string)base["serviceType"]; }
            set { base["serviceType"] = value; }
        }

        [ConfigurationProperty("serviceName", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceName
        {
            get { return (string)base["serviceName"]; }
            set { base["serviceName"] = value; }
        }

        [ConfigurationProperty("ip", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string IpAddress
        {
            get { return (string)base["ip"]; }
            set { base["ip"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = 0, IsKey = false, IsRequired = false)]
        public int Port
        {
            get { return (int)base["port"]; }
            set { base["port"] = value; }
        }
    }
}
