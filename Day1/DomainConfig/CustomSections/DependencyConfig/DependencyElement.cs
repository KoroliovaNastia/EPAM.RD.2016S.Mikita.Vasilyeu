using System.Configuration;

namespace DomainConfig.CustomSections.DependencyConfig
{
    public class DependencyElement : ConfigurationElement
    {
        [ConfigurationProperty("type", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string ServiceName
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
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