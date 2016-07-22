using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig
{
    [Serializable]
    public enum ServiceType
    {
        Master,
        Slave
    }
    [Serializable]
    public class ServiceConfiguration
    {
        public ServiceType Type { get; set; }
        public string Name { get; set; }
        public bool LoggingEnabled { get; set; }
        public string FilePath { get; set; }
    }
}
