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
    public class Receiver<T> : IDisposable
    {
        private Socket listener;
        private Socket reciever;
        public IPEndPoint IpEndPoint { get; set; }
        public Receiver(IPAddress ipAddress, int port)
        {
            IpEndPoint = new IPEndPoint(ipAddress, port);
            listener = new Socket(AddressFamily.InterNetwork,
               SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(IpEndPoint);
            listener.Listen(1);
        }

        public Task AcceptConnection()
        {
            return Task.Run(() =>
            {
                //Console.WriteLine("Wait Connection");
                reciever = listener.Accept();
                //Console.WriteLine("Connection accepted");
            });

        }

        public NetworkMessage<T> Receive()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NetworkMessage<T> message;

            using (var networkStream = new NetworkStream(reciever, false))
            {

                message = (NetworkMessage<T>)formatter.Deserialize(networkStream);
            }
            //Console.WriteLine("Message received!");
            return message;
        }

        public void Dispose()
        {
            reciever.Close();
            listener.Close();
        }
    }
}
