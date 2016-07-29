using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConfig
{
    [Serializable]
    public class Sender<T> : IDisposable
    {
        private IList<Socket> sockets = new List<Socket>();

        public void Connect(IEnumerable<IPEndPoint> ipEndPoints)
        {
            foreach (var ipEndPoint in ipEndPoints)
            {
                var socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipEndPoint);
                sockets.Add(socket);
            }

        }

        public void Connect(IPEndPoint ipEndPoint)
        {
            var socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipEndPoint);
            sockets.Add(socket);
        }

        public void Send(NetworkMessage<T> message)
        {
            foreach (var socket in sockets)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (NetworkStream networkStream = new NetworkStream(socket, false))
                {
                    formatter.Serialize(networkStream, message);
                }
            }
            //Console.WriteLine("Message is sent!");
        }

        public void Dispose()
        {
            foreach (var socket in sockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
