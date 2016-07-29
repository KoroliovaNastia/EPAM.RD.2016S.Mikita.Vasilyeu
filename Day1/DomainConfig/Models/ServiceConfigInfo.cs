using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DomainConfig
{
    [Serializable]
    public class ServiceConfigInfo
    {
        public ServiceType Type { get; set; }
        public string Name { get; set; }
        public IPEndPoint IpEndPoint { get; set; } 
    }

    [Serializable]
    public enum ServiceType
    {
        Master,
        Slave
    }
}
