using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConfig
{
    [Serializable]
    public class NetworkMessage<T>
    {
        public T Entity { get; set; }
        public MessageType MessageType { get; set; }
    }

    [Serializable]
    public enum MessageType
    {
        Added = 0,
        Deleted = 1
    }
}
