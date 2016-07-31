using System.Configuration;

namespace DomainConfig.CustomSections.ServiceConfig
{
    public class MasterServiceCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new SlaveServiceElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SlaveServiceElement)element).ServiceName;
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

        public SlaveServiceElement this[int idx] => (SlaveServiceElement)BaseGet(idx);
    }
}