using System.Configuration;

namespace DomainConfig.CustomSections.DependencyConfig
{
    public class MasterServiceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DependencyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DependencyElement)element).ServiceName;
        }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string MasterServiceName
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string MasterServiceType
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        public DependencyElement this[int idx] => (DependencyElement)BaseGet(idx);
    }
}